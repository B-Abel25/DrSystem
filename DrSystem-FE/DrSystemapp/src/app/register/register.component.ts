import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { RegisterModel } from '../models/registerModel';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerModel: RegisterModel = new RegisterModel();
  modalRef!: BsModalRef;
  constructor(private modalService: BsModalService, private accountService: AccountService) { }

  ngOnInit(): void {
  }
  register() {
    console.log('register called');
    this.accountService.register(this.registerModel);/*.subscribe((response: any) => {
      console.log(response);
    });
    console.log(this.registerModel)
*/
  }
}
