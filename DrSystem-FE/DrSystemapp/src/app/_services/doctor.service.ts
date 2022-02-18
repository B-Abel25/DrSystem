import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Client } from '../_models/client';

import { DoctorAdmin } from '../_models/doctorsadmin';
import { LostPassword } from '../_models/lostpasswordrequest';
import { NewPassword } from '../_models/newpassword';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
baseUrl= environment.apiUrl;
doctors!:DoctorAdmin[];
private currentDoctorSource= new ReplaySubject<DoctorAdmin>(1);
  currentDoctor$=this.currentDoctorSource.asObservable();
  constructor(private http: HttpClient) { }

  login(model:any)
  {
    return this.http.put<DoctorAdmin>(this.baseUrl + 'public/doctor/login', model).pipe(
      map((response: DoctorAdmin)=>{
        const doctor=response;
        if (doctor){
          this.setCurrentDoctor(doctor);
          localStorage.setItem('doctor', JSON.stringify(doctor));
        }
      })
    )
  }
  setCurrentDoctor(doctor: DoctorAdmin)
  {
    localStorage.setItem('doctor', JSON.stringify(doctor));
this.currentDoctorSource.next(doctor);
  }

  logout()
  {
    localStorage.removeItem('doctor');
    this.currentDoctorSource.next(null as any);
  }
  lostPassword(model:any){
    return this.http.put<LostPassword>(this.baseUrl+'public/lost-password', model).pipe(
      map((password:LostPassword)=>{
        if(password){
        localStorage.setItem('password', JSON.stringify(password));
        console.log(model)
        }
      })
    )
  }

  newPassword(model:any){
    return this.http.post<NewPassword>(this.baseUrl+'public/new-password', model).pipe(
      map((password:NewPassword)=>{
        if(password){
      
        console.log(model)
        }
      })
    )
  }

  getClients(){
    return this.http.get<Client[]>(this.baseUrl+'clients');
  }
  getClient(){
    return this.http.get<Client>(this.baseUrl+'client')
  }
}
  

