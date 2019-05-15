import { News } from '../news';
import { HttpClient } from '@angular/common/http';
import { Injectable, Component } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs';
import { NewsTitle } from '../NewsTitle';
@Injectable({
    providedIn: 'root',
})
@Component({
    providers: [Http]
})
export class ArticleService {
    constructor(private http: HttpClient) { }
  getNews(): Observable<News[][]> {
    return this.http.get<News[][]>('https://localhost:44398/api/Default/GetTitles');
    }
}
