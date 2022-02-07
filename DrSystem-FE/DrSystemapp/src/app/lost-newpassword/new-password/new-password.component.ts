import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-new-password',
  templateUrl: './new-password.component.html',
  styleUrls: ['./new-password.component.css']
})
export class NewPasswordComponent implements OnInit {
  newPasswordForm!:FormGroup;
  constructor(private fb:FormBuilder,private accountService:AccountService, private toatsr:ToastrService) { }

  ngOnInit() {
    this.initializeForm();
  }
  initializeForm(){
    this.newPasswordForm = this.fb.group({
     
     newpassword: ['', Validators.compose([Validators.required, Validators.minLength(8), Validators.maxLength(16)])],
     newconfirmPassword: ['', [Validators.required, this.matchValues('password')]],
     })
     this.newPasswordForm.controls['newpassword'].valueChanges.subscribe(()=>{
       this.newPasswordForm.controls['newconfirmPassword'].updateValueAndValidity();
     })
   }

   matchValues(matchTo:string) : ValidatorFn{
    return (control:AbstractControl | any )=>{
      return control?.value == control?.parent?.controls[matchTo].value ? null : {isMatching: true}
    }
  }
  newPassword(){
    console.log(this.newPasswordForm.value);
      // this.accountService.newPassword(this.newPasswordForm.value).subscribe(response=>{
      //   console.log(response);
       
      // }, error=>{
      //   this.validationErrors=error;
      //   console.log(error)
    
      // })
  }
}
