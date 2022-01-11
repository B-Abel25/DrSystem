import { Component, EventEmitter, OnInit, Output, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { LostPasswordModel } from '../models/lostPasswordModel';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-lostpassword',
  templateUrl: './lostpassword.component.html',
  styleUrls: ['./lostpassword.component.css']
})
export class LostpasswordComponent implements OnInit {
  
  @Output() lostPasswordOpenedEvent = new EventEmitter(); 
  paswwordLost: boolean = false
  lostPasswordModel: LostPasswordModel = new LostPasswordModel();
  modalRef!: BsModalRef;
  constructor(private modalService: BsModalService, private accountService: AccountService) { }

  
  
  ngOnInit(): void{
    
  }
  sendResetMail() {
    this.accountService.lostPassword(this.lostPasswordModel).subscribe((response: any) => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }
  openModal(template: TemplateRef<any>) {
  
    this.lostPasswordOpenedEvent.emit();
    
    this.modalRef = this.modalService.show(template);
  }

}
