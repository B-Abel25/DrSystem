import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent  {
  public model: any={}
  loggedIn: boolean = false;
  modalRef!: BsModalRef;
  loginForm:FormGroup;
  constructor(public accountService:AccountService, private router:Router, private toastr: ToastrService,private modalService: BsModalService, private fb:FormBuilder ) { }

  ngOnInit(): void {
   this.initializationForm();
    this.getCurrentUser();
  }
login(){
 
 this.accountService.login(this.loginForm.value).subscribe(response=>{
  this.router.navigateByUrl('/booking');
   this.loggedIn=true;
   }, error=>{
   console.log(error);
  
 })
}
logout()
{
  this.accountService.logout();
  this.router.navigateByUrl('/');
 
}
getCurrentUser(){
  this.accountService.currentUser$.subscribe(user=>{
    this.loggedIn=!!user;
  }, error=>{
console.log(error);
  });
  
}

openModal(template: TemplateRef<any>) {
  if (this.modalRef != null) {
    this.modalRef.hide();

  }
  this.modalRef = this.modalService.show(template);
}
closeModal() {
  console.log('force close')
  if (this.modalRef != null) {
    this.modalRef.hide();

  }
}
initializationForm(){
  this.loginForm=this.fb.group({
    medNumber: ['', [Validators.required, Validators.pattern('[0-9]{3}[0-9]{3}[0-9]{3}')], ],
    password: ['', Validators.compose([Validators.required, Validators.minLength(8), Validators.maxLength(16)])],
  })
}
}
