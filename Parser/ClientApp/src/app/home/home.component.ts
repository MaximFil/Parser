import { Component, OnInit } from '@angular/core';
import { ArticleService } from '../service/article.service';
import { finalize, catchError } from 'rxjs/operators';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalComponent } from '../modal/modal.component';
import { Site } from '../Site';
import { Observable, of } from 'rxjs';
import { Article } from '../Article';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  sites: Array<Site>;
  isLoaded: boolean;

  constructor(private articleService: ArticleService, private modalService: NgbModal) {
    this.sites = [];
    this.isLoaded = false;
  }

  ngOnInit() {
    this.getSites();
  }

  getSites(): void {
    this.articleService.getSites()
      .pipe(catchError(this.handleError<any>(Error)),
         finalize(() => {
          this.isLoaded = true;
        }))
      .subscribe(sites => (this.sites = sites));
  }

  getMoreArticles(siteId: number, siteNumber: number) {
    var partSiteArticles: Site;
    this.articleService.getMoreArticles(this.sites[siteNumber].idLastArticle, siteId)
      .pipe(catchError(this.handleError<any>(Error)),
         finalize(() => {
          for (let item of partSiteArticles.articles) {
            this.sites[siteNumber].articles.push(item);
          }
          this.sites[siteNumber].idLastArticle = partSiteArticles.idLastArticle;
        }))
      .subscribe(partSite => (partSiteArticles = partSite));
  }

  open(article: any) {
    const modalRef = this.modalService.open(ModalComponent);
    modalRef.componentInstance.title = article.title;
    modalRef.componentInstance.content = article.fullContent;
  }

  handleError<T>(result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
  }

  deleteArticle(idArticle: number, numberSite:number,numberArticle:number, idLastArticle:number, idSite) {
    this.articleService.deleteArticle(idArticle).subscribe();
    this.getArticle(numberSite, numberArticle, idLastArticle, idSite);
  }

  getArticle(numberSite: number, numberArticle: number, idLastArticle: number, idSite: number) {
    var siteArticle: Article;
    this.sites[numberSite].articles.splice(numberArticle, 1);
    this.articleService.getArticle(idSite, idLastArticle)
      .pipe(catchError(this.handleError<any>(Error)),
        finalize(() => {
          if (siteArticle.link.length > 0) {
            this.sites[numberSite].articles.push(siteArticle);
            this.sites[numberSite].idLastArticle = siteArticle.id;
          }
        }))
      .subscribe(article => siteArticle = article);       
  }
}
