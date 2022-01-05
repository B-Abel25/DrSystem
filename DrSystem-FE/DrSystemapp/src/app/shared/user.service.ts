import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private fb: FormBuilder, private http: HttpClient) { }
  readonly BaseURI = 'localhost:44347';

  formModel = this.fb.group({
    FullName:['', Validators.required],
    TAJnumber: ['', Validators.required],
    Email: ['', Validators.email],
   PhoneNumber:['', Validators.required],
    Passwords: this.fb.group({
      Password: ['', [Validators.required, Validators.minLength(4)]],
      ConfirmPassword: ['', Validators.required]
    }, { validator: this.comparePasswords })

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
      FullName: this.formModel.value.FullName,
      TAJnumber: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      PhoneNumber: this.formModel.value.PhoneNumber,
      Password: this.formModel.value.Passwords.Password
    };
    return this.http.post(this.BaseURI + '/public/register', body);
  }

  login(formData: any) {
    return this.http.post(this.BaseURI + '/public/login', formData);
  }

  getUserProfile() {
    return this.http.get(this.BaseURI + '/public/lost-password');
  }
}
