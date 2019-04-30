import { News } from './news';
import { HttpClient } from '@angular/common/http';
import { Injectable, Component } from '@angular/core';
import { Http } from '@angular/http';
import { Observable, of } from 'rxjs';


@Injectable({
  providedIn:'root',
})
@Component({
  providers: [Http]
}) 

export class HabrService {
  public ArticlesLinks: News[];
  constructor(private http: HttpClient) { }
  getNews(): Observable<Array<News>> {

    return this.http.get<Array<News>>('https://localhost:44398/api/Default/GetTitleArticles');

  }
}
