import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-modal',
  templateUrl: './modal-tamplate.html',
  styleUrls: ['./modal-template.css']
})
export class ModalComponent implements OnInit {

  @Input() title: string;
  @Input() content: string;

  constructor(
    private activeModal: NgbActiveModal
  ) { }

  ngOnInit() {
  }
}
