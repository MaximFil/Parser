import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ArticleService } from '../service/article.service';
import { NameSite } from '../NameSite';
@Component({
    selector: 'modal-show',
    templateUrl: './modal-filters-template.html',
    styleUrls: ['./modal-filters-template.css']
})
export class ModalFiltersComponent implements OnInit {
  nameSitesUser: Array<NameSite>;
  constructor(private activeModal: NgbActiveModal, private articleService: ArticleService) { }
  ngOnInit() {
    this.getNameSitesUser();
  }
  getNameSitesUser() {
    this.articleService.getNameSitesUser()
      .subscribe(nameSitesUser => this.nameSitesUser = nameSitesUser);
  }
}
