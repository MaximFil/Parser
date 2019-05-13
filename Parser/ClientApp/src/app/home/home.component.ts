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
  ArticlesLinksAll: NewsTitle[];
  ArticlesLinksPart: Array<News>;
  Articles: Array<News>;
  isLoading: boolean = false;
  countAllNews: number;
  countPartNews: number = 0;
  constructor(private ArticleService: ArticleService, private modalService: NgbModal) {
    this.ArticlesLinksAll = [];
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
          this.getPartNews();
          this.countAllNews = this.ArticlesLinksAll[0].news.length;
          this.isLoading = true;
        }))
      .subscribe(ArticlesLinks => (this.ArticlesLinksAll = ArticlesLinks));
      
  }

  getPartNews(): void {
    //вот здесь
    this.Articles=this.ArticlesLinksAll[0].news.slice(0, 9 * (this.countPartNews + 1));
    this.countPartNews++;
  }
  open(Article: any) {
    const modalRef = this.modalService.open(ModalComponent);
    modalRef.componentInstance.article = Article.article;
    modalRef.componentInstance.content = Article.fullContent;
  }
}

