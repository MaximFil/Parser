import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IWebDriverCookie, IWebElement, error } from 'selenium-webdriver';
//import { ArticleLink } from './ArticleLink';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public ArticlesLinks: ArticleLink[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<ArticleLink[]>(baseUrl + 'api/Default/GetTitleArticles').subscribe(result => {
      this.ArticlesLinks = result;
    }, error => console.error(error));
  }
}
interface ArticleLink {
  article: string;
  link: string;
  content: string;
}
