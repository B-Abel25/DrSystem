import { Component, EventEmitter, HostListener, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalContainerComponent } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';

import { LostPassword } from 'src/app/_models/lostpasswordrequest';
import { Registration } from 'src/app/_models/registration';
import { AccountService } from 'src/app/_services/account.service';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-lost-password-request',
  templateUrl: './lost-password-request.component.html',
  styleUrls: ['./lost-password-request.component.css']
})
export class LostPasswordRequestComponent implements OnInit {



  requestForm!: NgForm;
  lostPasswordForm: FormGroup;
  @Output() lostPasswordOpenedEvent = new EventEmitter();
  paswwordLost: boolean = false
  public model: any = {}
  editForm: any;


  modalRef!: BsModalRef;
  constructor(private modalService: BsModalService, private accountService: AccountService
    , private toastr: ToastrService, private router: Router, private fb: FormBuilder) {

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

  initializationForm() {
    this.lostPasswordForm = this.fb.group({
      userNumber: ['', [Validators.required, Validators.pattern('[0-9]{3}[0-9]{3}[0-9]{3}')],],

    })
  }
  public Close() {
   
    this.modalRef.hide();
    this.lostPasswordForm.reset();
  }
  closeModal() {
   
    if (this.modalRef != null) {
      this.modalRef.hide();
      this.lostPasswordForm.reset();
      
    }
  }

}
