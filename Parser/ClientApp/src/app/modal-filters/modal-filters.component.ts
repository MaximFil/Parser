import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
    selector: 'modal-show',
    templateUrl: './modal-filters-template.html',
    styleUrls: ['./modal-filters-template.css']
})
export class ModalFiltersComponent implements OnInit {
    constructor(private activeModal: NgbActiveModal) { }
    ngOnInit() {
    }
}
