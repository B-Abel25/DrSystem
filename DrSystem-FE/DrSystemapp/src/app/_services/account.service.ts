import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Doctor } from '../_models/doctor';
import { LostPassword } from '../_models/lostpasswordrequest';
import { NewPassword } from '../_models/newpassword';
import { Place } from '../_models/place';
import { Registration } from '../_models/registration';




@Injectable({
  providedIn: 'root'
})
export class AccountService {


  baseUrl=environment.apiUrl;


  private currentClientSource= new ReplaySubject<Registration>(1);
  currentClient$=this.currentClientSource.asObservable();
 id!:Registration;
  constructor(private http:HttpClient) { }


  login(model:any)
  {

    return this.http.post<Registration>(this.baseUrl + 'public/client/login', model).pipe(
      map((response: Registration) => {
        const client = response;
        if (client) {
          this.setCurrentClient(client);
          localStorage.setItem('client', JSON.stringify(client));
        }
      })
    )
  }
  register(model: any) {
    return this.http.post<Registration>(this.baseUrl + 'public/client/register', model).pipe(
      map((client: Registration) => {
        if (client) {
          this.setCurrentClient(client);

          this.currentClientSource.next(client);
          console.log(model);
        }


      })

    )
  }
  lostPassword(model: any) {
    return this.http.put<LostPassword>(this.baseUrl + 'public/lost-password', model).pipe(
      map((password: LostPassword) => {
        if (password) {
          localStorage.setItem('password', JSON.stringify(password));
          console.log(model)
        }
      })
    )
  }

  newPassword(model: any) {
    return this.http.post<NewPassword>(this.baseUrl + 'public/new-password', model).pipe(
      map((password: NewPassword) => {
        if (password) {

          console.log(model)
        }
      })
    )
  }
  setCurrentClient(client: Registration) {
    localStorage.setItem('client', JSON.stringify(client));
    this.currentClientSource.next(client);
  }

  logout() {
    localStorage.clear();
    this.currentClientSource.next(null as any);
  }

  getPlaces() {
    return this.http.get<Place[]>(this.baseUrl + 'public/register/places')
  }
  getDoctors() {
    return this.http.get<Doctor[]>(this.baseUrl + 'public/register/doctors')
  }
}
