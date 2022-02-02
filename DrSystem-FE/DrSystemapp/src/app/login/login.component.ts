import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent  {
  public model: any={}
  loggedIn: boolean = false;
  modalRef!: BsModalRef;
  constructor(public accountService:AccountService, private router:Router, private toastr: ToastrService,private modalService: BsModalService ) { }

  ngOnInit(): void {
   
    this.getCurrentUser();
  }
login(){
 this.accountService.login(this.model).subscribe(response=>{
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
}
