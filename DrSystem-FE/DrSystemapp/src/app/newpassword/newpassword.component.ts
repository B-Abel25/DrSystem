import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NewPasswordModel } from '../models/newPasswordModel';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-newpassword',
  templateUrl: './newpassword.component.html',
  styleUrls: ['./newpassword.component.css']
})
export class NewpasswordComponent implements OnInit {
  newPasswordModel: NewPasswordModel = new NewPasswordModel();
  token!: any
  constructor(private route: ActivatedRoute, private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.token = this.route.snapshot.paramMap.get('token')
  }
  setNewPassword() {
    this.newPasswordModel.token = this.token;
    console.log(this.token)
    this.accountService.setNewPassword(this.newPasswordModel).subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
    this.router.navigate(['/'])
  }
}
