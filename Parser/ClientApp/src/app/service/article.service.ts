import { Article } from '../Article';
import { HttpClient } from '@angular/common/http';
import { Injectable, Component } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs';
import { Site } from '../Site';
import { NameSite } from '../NameSite';
@Injectable({
    providedIn: 'root',
})
@Component({
    providers: [Http]
})
export class ArticleService {
  constructor(private http: HttpClient) { }

  getNews(): Observable<Array<Site>>
  {
    return this.http.get<Array<Site>>('https://localhost:44398/api/Parser/GetSites');
  }

  getPartNews(idLastArticle: number, siteNumber: number): Observable<Site>
  {
    return this.http.get<Site>('https://localhost:44398/api/Parser/GetPartSites?idLastArticle=' + idLastArticle + '&siteNumber=' + siteNumber);
  }
  getNameSitesUser(): Observable<Array<NameSite>> {
    return this.http.get<Array<NameSite>>('https://localhost:44398/api/Help/GetNameSitesUser');
  }
}
