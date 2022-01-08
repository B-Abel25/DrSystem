import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { LoginModel } from '../models/loginModel';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent  {
  loggedIn: boolean = false
  loginModel: LoginModel = new LoginModel();
  modalRef!: BsModalRef;
  constructor(private modalService: BsModalService, private accountService: AccountService) { }

  
  login() {
    
    // this.loggedIn = true
    // this.loggedInEvent.emit(this.loggedIn);
    this.accountService.login(this.loginModel).subscribe((response: any) => {
      console.log(response);
      this.loggedIn = true;
      this.accountService.handleLogin(true);
    }, (error: any) => {
      console.log(error);
    });

  
    // console.log('login with ' + this.loginModel.username)
  }

  logout() {
    this.loggedIn = false;

    this.accountService.handleLogin(false);
    // this.loggedIn = false
    // this.loggedInEvent.emit(this.loggedIn);
    // console.log('logging out')
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
