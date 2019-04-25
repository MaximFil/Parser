import { Component, OnInit } from '@angular/core';
import { HabrService } from './habr.service';
import { News } from './news';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  ArticlesLinks: Array<News> = [];

  constructor(private habrService: HabrService) { }

  ngOnInit() {
    this.getNews();
  }
  getNews(): void {
    this.habrService.getNews().subscribe(ArticlesLinks => (this.ArticlesLinks = ArticlesLinks));
  }
}

