import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AdminLoginModel } from '../models/adminLoginModul';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-admin-login',
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.css']
})
export class AdminLoginComponent implements OnInit {

  adminloggedIn: boolean = false
  adminLoginModel: AdminLoginModel = new AdminLoginModel();
  modalRef!: BsModalRef;
  constructor(private modalService: BsModalService, private accountService: AccountService) { }
  ngOnInit(): void {
   
  }

  
  adminlogin() {
    
    // this.loggedIn = true
    // this.loggedInEvent.emit(this.loggedIn);
    this.accountService.adminlogin(this.adminLoginModel).subscribe((response: any) => {
      console.log(response);
      this.adminloggedIn = true;
      this.accountService.handleLogin(true);
    }, (error: any) => {
      console.log(error);
    });

  
    // console.log('login with ' + this.loginModel.username)
  }

  logout() {
    this.adminloggedIn = false;

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
