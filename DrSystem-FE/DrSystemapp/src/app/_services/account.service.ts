import { HttpClient, HttpHandler, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Doctors } from '../_models/doctor';
import { LostPassword } from '../_models/lostpasswordrequest';
import { User } from '../_models/user';



@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl=environment.apiUrl;
  private currentUserSource= new ReplaySubject<User>(1);
  currentUser$=this.currentUserSource.asObservable();

  constructor(private http:HttpClient) { }

  login(model:any)
  {
    return this.http.post<User>(this.baseUrl + 'public/login', model).pipe(
      map((response: User)=>{
        const user=response;
        if (user){
          this.setCurrentUser(user);
          
        }
      })
    )
  }
  register(model: any){
return this.http.post<User>(this.baseUrl + 'public/register',model).pipe(
  map((user: User)=>{
    if(user){
     this.setCurrentUser(user);
      this.currentUserSource.next(user);
      console.log(model);
    }
   
  })
)
  }
  lostPassword(model:any){
    return this.http.post<LostPassword>(this.baseUrl+'public/lost-password', model).pipe(
      map((password:LostPassword)=>{
        if(password){
        localStorage.setItem('password', JSON.stringify(password));
        console.log(model)
        }
      })
    )
  }
  setCurrentUser(user: User)
  {
    localStorage.setItem('user', JSON.stringify(user));
this.currentUserSource.next(user);
  }

  logout()
  {
    localStorage.removeItem('user');
    this.currentUserSource.next(null as any);
  }

  getDoctors(){
    return this.http.get<Doctors[]>(this.baseUrl+'public/doctors')
  }
}
