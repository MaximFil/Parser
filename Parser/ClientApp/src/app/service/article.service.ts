import { ArticleViewModel } from '../ArticleViewModel';
import { HttpClient } from '@angular/common/http';
import { Injectable, Component } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs';
import { SiteViewModel } from '../SiteViewModel';
@Injectable({
    providedIn: 'root',
})
@Component({
    providers: [Http]
})
export class ArticleService {
  constructor(private http: HttpClient) { }
  getNews(numberSite: number): Observable<Array<SiteViewModel>> {
    return this.http.get<Array<SiteViewModel>>('https://localhost:44398/api/Parser/GetSites?numberPart=' + numberSite);
    }
}
