import { Component } from '@angular/core';
import { ModalFiltersComponent } from '../modal-filters/modal-filters.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  constructor(private modalService: NgbModal) {}

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
  open() {
    const modalRef = this.modalService.open(ModalFiltersComponent);
  }
}
