import { Component, OnInit, Input } from '@angular/core';
import { ArticleService } from '../service/article.service';
import { Article } from '../Article';
import { MatProgressSpinner } from '@angular/material';
import { Observable, of } from 'rxjs';
import { finalize, catchError } from 'rxjs/operators';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalComponent } from '../modal/modal.component';
import { Site } from '../Site';
import { error } from '@angular/compiler/src/util';
import { log } from 'util';
import { Title } from '@angular/platform-browser';
import { Content } from '@angular/compiler/src/render3/r3_ast';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  siteArticles: Array<Site>;
  partSiteArticles: Site;
  isLoading: boolean = false;
  countAllNews: number;
  constructor(private ArticleService: ArticleService, private modalService: NgbModal) {
    this.siteArticles = [];
  }

  ngOnInit() {
    this.getNews();
  }

  getNews(): void {
    this.ArticleService.getNews()
      .pipe(catchError(error => {
        console.log('error occured:', error);
        throw error;
      })
        , finalize(() => {
          this.countAllNews = this.siteArticles.length;
          this.isLoading = true;
        }))
      .subscribe(ArticlesLinks => (this.siteArticles = ArticlesLinks));
  }
  getPartNews(siteNumber: number) {
    var ber: number = siteNumber;
    this.ArticleService.getPartNews(this.siteArticles[ber].idLastArticle, ++siteNumber)
      .pipe(catchError(error => {
        console.log('error occured:', error);
        throw error;
      })
        , finalize(() => {
          for (let item of this.partSiteArticles.articles) {
            this.siteArticles[ber].articles.push(item);
          }
          this.siteArticles[ber].idLastArticle = this.partSiteArticles.idLastArticle;
        }))
      .subscribe(ArticlesLinks => (this.partSiteArticles = ArticlesLinks));
  }
  open(Article: any) {
    const modalRef = this.modalService.open(ModalComponent);
    modalRef.componentInstance.title = Article.title;
    modalRef.componentInstance.content = Article.fullContent;
  }
}
