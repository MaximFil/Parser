import { Component, OnInit, Input } from '@angular/core';
import { ArticleService } from '../service/article.service';
import { News } from '../news';
import { MatProgressSpinner } from '@angular/material';
import { Observable, of } from 'rxjs';
import { finalize, catchError } from 'rxjs/operators';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalComponent } from '../modal/modal.component';
import { NewsTitle } from '../NewsTitle';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  ArticlesLinksAll: News[][];
  ArticlesLinksPart: News[][];
  Articles: Array<News>;
  isLoading: boolean = false;
  countAllNews: number;
  countPartNews: number[]=[];
  constructor(private ArticleService: ArticleService, private modalService: NgbModal) {
    this.ArticlesLinksAll = [];
    this.ArticlesLinksPart = [];
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
          this.countAllNews = this.ArticlesLinksAll.length;
          this.getPartNews();
          this.isLoading = true;
        }))
      .subscribe(ArticlesLinks => (this.ArticlesLinksAll = ArticlesLinks));

  }

  getPartNews(): void {
    for (var i = 0; i < this.countAllNews; i++) {
      this.ArticlesLinksPart[i] = this.ArticlesLinksAll[i].slice(0, 9);
      this.countPartNews[i] = 0;
      //this.countPartNews++;
    }
  }
  getPartNewsStill(number: number): void {
    this.ArticlesLinksPart[number] = this.ArticlesLinksAll[number].slice(0, 9 * (++this.countPartNews[number] + 1));
  }
  open(Article: any) {
    const modalRef = this.modalService.open(ModalComponent);
    modalRef.componentInstance.article = Article.article;
    modalRef.componentInstance.content = Article.fullContent;
  }
}
