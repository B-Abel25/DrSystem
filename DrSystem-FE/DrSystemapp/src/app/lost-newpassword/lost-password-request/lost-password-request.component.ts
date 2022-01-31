import { Component, EventEmitter, HostListener, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';

import { LostPassword } from 'src/app/_models/lostpasswordrequest';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-lost-password-request',
  templateUrl: './lost-password-request.component.html',
  styleUrls: ['./lost-password-request.component.css']
})
export class LostPasswordRequestComponent implements OnInit {
  @ViewChild('requestForm')
  requestForm!: NgForm ;
  @Output() lostPasswordOpenedEvent = new EventEmitter(); 
  paswwordLost: boolean = false
  public model: any={}
  editForm: any;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any){
    if (this.requestForm.dirty) {
      $event.returnValue=true;
    }
  }
  modalRef!: BsModalRef;
  constructor(private modalService: BsModalService, private accountService: AccountService, private toastr:ToastrService) {

  }

  sendResetMail() {
    this.accountService.lostPassword(this.model).subscribe(response => {
      console.log(response);
     
    }, error => {
      console.log(error);
    });
  }

  ngOnInit(): void {
  }

  openModal(template: TemplateRef<any>) {
  
    this.lostPasswordOpenedEvent.emit();
    
    this.modalRef = this.modalService.show(template);
  }


}
