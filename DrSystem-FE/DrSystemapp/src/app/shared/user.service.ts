import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { RegisterModel } from '../model/registerModel';
import { LostPasswordModel } from '../model/lostPasswordModel';
import { LoginModel } from '../model/loginModel';
import { catchError, map, retry } from 'rxjs/operators';

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

  getUserProfile() {
    return this.http.get(this.BaseURI + '/public/lost-password');
  }

  urlBase : string = "http://localhost:44347";
  registerUrl : string = this.urlBase +  "/public/register";
  loginUrl : string = this.urlBase +  "/public/login";
  lostPasswordUrl : string = this.urlBase +  "/public/lost-password";

  login(body: LoginModel) {
    console.log('login called');
    return this.callPostBackend(body, this.loginUrl);
  }
  
  lostPassword(body: LostPasswordModel) {
    console.log('lost-password called');
    return this.callPostBackend(body, this.lostPasswordUrl);
  }

  register(body: RegisterModel) {
    console.log('register called');
    return this.callPostBackend(body, this.registerUrl);
  }

  callPostBackend(body: Object, url: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })
    };

    const bodyJson : string = JSON.stringify(body);
    console.log(bodyJson);
    return this.http.post(url, body).pipe(map((response: Object) => {
      console.log('post called with response: ' + JSON.stringify(response));
      return response;
    }));
  }

  callPutBackend(body: Object, url: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })
    };

    const bodyJson : string = JSON.stringify(body);
    console.log(bodyJson);
    return this.http.put(url, body).pipe(map((response: Object) => {
      console.log('put called with response: ' + JSON.stringify(response));
      return response;
    }));
  }
}
