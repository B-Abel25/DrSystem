import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { map } from 'rxjs';
import { LoginModel } from '../models/loginModel';
import { RegisterModel } from '../models/registerModel';
import { Axios } from 'axios';

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
  
   urlBase : string = "http://localhost:44347";
   registerUrl : string = this.urlBase +  "/public/register";
   loginUrl : string = this.urlBase +  "/public/login";
  // lostPasswordUrl : string = this.urlBase +  "/public/lost-password";
  // newPasswordUrl : string = this.urlBase +  "/public/new-password";
 
   login(body: LoginModel) {

     console.log('login called');
     return this.callPostBackend(body, this.loginUrl);
   }
 
   register(body: RegisterModel) {
     console.log('register called');
     console.log(this.registerUrl);
     return this.callPostBackend(body, this.registerUrl);
   }
 
   callPostBackend(body: Object, url: string) {
     
      const httpOptions = {
        headers: new HttpHeaders({
          'Content-Type':  'application/json'
        })
      };
  /*
      const bodyJson : string = JSON.stringify(body);
      console.log(bodyJson);
      return this.http.post(url, body).pipe(map((response: Object) => {
        console.log('post called with response: ' + JSON.stringify(response));
        console.log(response);
        return response;
      }));
*/
      const axios = require('axios').default;
      axios.post(url,body)
      .then(function (response: Object){
        // handle success
        console.log(response);
      });
      
      
   }
 //TODO dlsfikjhdsl
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
