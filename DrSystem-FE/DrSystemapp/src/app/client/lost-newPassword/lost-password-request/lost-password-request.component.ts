import { Component, EventEmitter, HostListener, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';

import { LostPassword } from 'src/app/_models/lostpasswordrequest';
import { Registration } from 'src/app/_models/registration';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-lost-password-request',
  templateUrl: './lost-password-request.component.html',
  styleUrls: ['./lost-password-request.component.css']
})
export class LostPasswordRequestComponent implements OnInit {
  @ViewChild('requestForm')
  requestForm!: NgForm ;
  lostPasswordForm:FormGroup;
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
  constructor(private modalService: BsModalService, private accountService: AccountService
    , private toastr:ToastrService, private router:Router, private fb:FormBuilder) {

  }

  sendResetMail() {
    this.accountService.lostPassword(this.lostPasswordForm.value).subscribe(response => {
      console.log(response);
     
    }, error => {
      console.log(error);
    });
  }

  ngOnInit() {
    this.initializationForm();
  }

  openModal(template: TemplateRef<any>) {
  
    this.lostPasswordOpenedEvent.emit();
    
    this.modalRef = this.modalService.show(template);
  }
  initializationForm(){
    this.lostPasswordForm=this.fb.group({
      medNumber: ['', [Validators.required, Validators.pattern('[0-9]{3}[0-9]{3}[0-9]{3}')], ],
     
    })
  }

}
