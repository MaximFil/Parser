import { HttpClient } from '@angular/common/http';
import { Injectable, Component } from '@angular/core';
import { Http, ResponseType } from '@angular/http';
import { Observable } from 'rxjs';
import { Site } from '../Site';
import { NameSite } from '../NameSite';
import { Article } from '../Article';
@Injectable({
    providedIn: 'root',
})
@Component({
    providers: [Http]
})
export class ArticleService {
  constructor(private http: HttpClient) { }

  getSites(): Observable<Array<Site>>
  {
    return this.http.get<Array<Site>>('https://localhost:44398/api/Parser/GetSites');
  }

  getMoreArticles(idLastArticle: number, siteId: number): Observable<Site>
  {
    return this.http.get<Site>('https://localhost:44398/api/Parser/getMoreArticles?idLastArticle=' + idLastArticle + '&siteId=' + siteId);
  }

  getNameSitesUser(): Observable<Array<NameSite>> {
    return this.http.get<Array<NameSite>>('https://localhost:44398/api/Help/GetNameSitesUser');
  }

  getValueShow(): Observable<boolean> {
    return this.http.get<boolean>('https://localhost:44398/api/Help/GetValueShow');
  }

  saveFilters(nameSite: Array<NameSite>, showArticle: boolean) {
    return this.http.post('https://localhost:44398/api/Help/SaveFilters?showArticles=' + showArticle, nameSite, { responseType: 'text' });
  }

  deleteArticle(idArticle: number) {
    return this.http.post('https://localhost:44398/api/Help/DeleteArticle', idArticle, { responseType: 'text' });
  }

  getArticle(idSite: number, idLastArticle: number): Observable<Article> {
    return this.http.get<Article>('https://localhost:44398/api/Help/GetArticle?idLastArticle=' + idLastArticle+'&idSite='+idSite);
  }
  saveShowenArticle(articleId: number) {
    return this.http.post('https://localhost:44398/api/Help/SaveShowenArticle', articleId, { responseType:'text' });
}
}
