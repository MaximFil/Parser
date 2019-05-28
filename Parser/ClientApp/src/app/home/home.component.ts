import { Component, OnInit, Input } from '@angular/core';
import { ArticleService } from '../service/article.service';
import { ArticleViewModel } from '../ArticleViewModel';
import { MatProgressSpinner } from '@angular/material';
import { Observable, of } from 'rxjs';
import { finalize, catchError } from 'rxjs/operators';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalComponent } from '../modal/modal.component';
import { SiteViewModel } from '../SiteViewModel';
import { error } from '@angular/compiler/src/util';
import { log } from 'util';
import { Title } from '@angular/platform-browser';
import { Content } from '@angular/compiler/src/render3/r3_ast';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  siteArticles: Array<SiteViewModel>;
  isLoading: boolean = false;
  countAllNews: number;
  constructor(private ArticleService: ArticleService, private modalService: NgbModal) {
    this.siteArticles = [];
  }

  ngOnInit() {
    this.getNews(3);
  }

  getNews(numberPage: number): void {
    this.ArticleService.getNews(numberPage)
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
  open(Article: any) {
    const modalRef = this.modalService.open(ModalComponent);
    modalRef.componentInstance.title = Article.title;
    modalRef.componentInstance.content = Article.fullContent;
  }
}
