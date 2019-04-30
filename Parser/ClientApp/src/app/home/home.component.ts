import { Component, OnInit } from '@angular/core';
import { HabrService } from './habr.service';
import { News } from './news';
import { MatProgressSpinner } from '@angular/material';
import { Observable, of } from 'rxjs';
import { finalize, catchError } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  ArticlesLinksAll: Array<News>;
  ArticlesLinksPart: Array<News>;
  isLoading: boolean = false;
  countAllNews: number;
  countPartNews: number = 0;
  constructor(private habrService: HabrService) { }

  ngOnInit() {
    this.getNews();
  }

  getNews(): void {
    this.habrService.getNews()
      .pipe(catchError(error => {
        console.log('error occured:', error);
        throw error;
      })
        , finalize(() => {
          this.getPartNews();
          this.countAllNews = this.ArticlesLinksAll.length;
          this.isLoading = true;
        }))
      .subscribe(ArticlesLinks => (this.ArticlesLinksAll = ArticlesLinks));
  }

  getPartNews(): void {
    this.ArticlesLinksPart = this.ArticlesLinksAll.slice(0, 9 * (this.countPartNews + 1));
    this.countPartNews++;
  }
}

