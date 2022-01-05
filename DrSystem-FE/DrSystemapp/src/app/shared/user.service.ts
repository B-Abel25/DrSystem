import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private fb: FormBuilder, private http: HttpClient) { }
  readonly BaseURI = 'http://localhost:44347';

  formModel = this.fb.group({
      FullName:['', Validators.required],
      TAJnumber: ['', Validators.required],
      Email: ['', Validators.email],
      PhoneNumber:['', Validators.required],
      Password: ['', [Validators.required, Validators.minLength(4)]],
      ConfirmPassword: ['', Validators.required]
    });

  comparePasswords(fb: FormGroup) {
    let confirmPswrdCtrl = fb.get('ConfirmPassword');
    //passwordMismatch
    //confirmPswrdCtrl.errors={passwordMismatch:true}
    if (confirmPswrdCtrl?.getError == null || 'passwordMismatch' in confirmPswrdCtrl.getError) {
      if (fb.get('Password') != confirmPswrdCtrl)
        confirmPswrdCtrl?.setErrors({ passwordMismatch: true });
      else
        confirmPswrdCtrl?.setErrors(null);
    }
  }

  register() {
    var body = {
      name: this.formModel.value.FullName,
      medNumber: this.formModel.value.TAJnumber,
      email: this.formModel.value.Email,
      phoneNumber: this.formModel.value.PhoneNumber,
      password: this.formModel.value.Password
    };
    console.log(body);
    return this.http.post(this.BaseURI + '/public/register', body);
  }

  login(formData: any) {
    return this.http.post(this.BaseURI + '/public/login', formData);
  }

  getUserProfile() {
    return this.http.get(this.BaseURI + '/public/lost-password');
  }
}
