import { Component, Input, OnInit } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-modal',
  templateUrl: './modal-tamplate.html',
  styleUrls: ['./modal-template.css']
})
export class ModalComponent implements OnInit {

  @Input() article: `frferfe`;
  @Input() content: `frefrefe`;

  constructor(
    private activeModal: NgbActiveModal
  ) { }

  ngOnInit() {
  }

}
