import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ArticleService } from '../service/article.service';
import { NameSite } from '../NameSite';
import { HomeComponent } from '../home/home.component';
@Component({
    selector: 'modal-show',
    templateUrl: './modal-filters-template.html',
    styleUrls: ['./modal-filters-template.css']
})
export class ModalFiltersComponent implements OnInit {
  nameSitesUser: Array<NameSite>;
  mas: Array<number>
  homeComponent: HomeComponent;
  showArticle: boolean;
  constructor(private activeModal: NgbActiveModal, private articleService: ArticleService) { }
  ngOnInit() {
    this.getValueShow();
    this.getNameSitesUser();   
  }
  getNameSitesUser() {
    this.articleService.getNameSitesUser()
      .subscribe(nameSitesUser => this.nameSitesUser = nameSitesUser);
  }
  saveFilters() {
    
    this.articleService.saveFilters(this.nameSitesUser, this.showArticle).subscribe();
    this.activeModal.close();
   // this.homeComponent.getNews();
    window.location.reload();
  }
  getValueShow() {
    this.articleService.getValueShow().subscribe(showArticle => this.showArticle = showArticle);
  }
}
