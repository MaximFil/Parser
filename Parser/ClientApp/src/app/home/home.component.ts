import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IWebDriverCookie, IWebElement, error } from 'selenium-webdriver';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public links: IWebElement[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<IWebElement[]>(baseUrl + 'api/Default/TitleArticles').subscribe(result => {
      this.links = result;
    }, error => console.error(error));
  }
}
