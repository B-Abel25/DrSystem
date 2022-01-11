import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { map } from 'rxjs';
import { LoginModel } from '../models/loginModel';
import { LostPasswordModel } from '../models/lostPasswordModel';
import { NewPasswordModel } from '../models/newPasswordModel';
import { RegisterModel } from '../models/registerModel';


@Injectable({
  providedIn: 'root'
})
export class AccountService {
  
  
  @Output() loggedInEvent = new EventEmitter();
  users: any
  loggedIn: boolean = false

  constructor(private http: HttpClient) { }
  handleLogin(state: boolean) {
    this.loggedIn = state
    this.loggedInEvent.emit(this.loggedIn);
 
   }
  
   urlBase : string = 'https://localhost:5001/';
   registerUrl : string = this.urlBase +  'public/register';
   loginUrl : string = this.urlBase +  'public/login';
   lostPasswordUrl : string = this.urlBase +  "/public/lost-password";
  newPasswordUrl : string = this.urlBase +  "/public/new-password";
 
   login(body: LoginModel) {

     console.log('login called');
     return this.callPostBackend(body, this.loginUrl);
   }
 
   register(body: RegisterModel) {
     console.log('register called');
     console.log(this.registerUrl);
     return this.callPostBackend(body, this.registerUrl);
   }
   lostPassword(body: LostPasswordModel) {
    console.log('lost-password called');
    return this.callPostBackend(body, this.lostPasswordUrl);
  }
  setNewPassword(body: NewPasswordModel) {
    console.log('new-password called wtih token: ' + body.token);
    return this.callPutBackend(body, this.newPasswordUrl);
  }
   
 
   callPostBackend(body: Object, url: string) {
     
      const httpOptions = {
        headers: new HttpHeaders({
          'Content-Type':  'application/json'
        })
      };
  console.log(url);
      const bodyJson : string = JSON.stringify(body);
      console.log(bodyJson);
      return this.http.post(url, body).pipe(map((response: Object) => {
        console.log('post called with response: ' + JSON.stringify(response));
        console.log(response);
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
