import { HttpClient } from '@angular/common/http';
import { identifierModuleUrl } from '@angular/compiler';
import { Injectable } from '@angular/core';

import { map, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Client } from '../_models/client';
import { Doctor } from '../_models/doctor';

import { LostPassword } from '../_models/lostpasswordrequest';
import { NewPassword } from '../_models/newpassword';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {


  baseUrl = environment.apiUrl;
  //doctors!: DoctorAdmin[];
  private currentDoctorSource = new ReplaySubject<Doctor>(1);
  currentDoctor$ = this.currentDoctorSource.asObservable();
  singleuserdata: Client[];
  constructor(private http: HttpClient) { }

  login(model: any) {

    return this.http.put<Doctor>(this.baseUrl + 'public/doctor/login', model).pipe(
      map((response: Doctor) => {
        const doctor = response;
        if (doctor) {
          this.setCurrentDoctor(doctor);
          localStorage.setItem('doctor', JSON.stringify(doctor));
        }
      })
    );
  }
  setCurrentDoctor(doctor: Doctor) {
    localStorage.setItem('doctor', JSON.stringify(doctor));
    this.currentDoctorSource.next(doctor);
  }

  logout() {
    localStorage.removeItem('doctor');
    this.currentDoctorSource.next(null as any);
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
  getDoctorClientsRequest() {
    return this.http.get<Client[]>(this.baseUrl + 'private/doctor/clients-request');
  }
  getDoctorClients() {
    return this.http.get<Client[]>(this.baseUrl + 'private/doctor/clients');
  }

  deleteClient(clientId: string) {

    return this.http.delete(this.baseUrl + 'private/doctor/client-request/decline/' + clientId).subscribe({
      next: data => {
        console.log(data)
      },
      error: error => {

        console.error('There was an error!', error);
      }
    });

  }
  acceptClient(clientId: string) {


    return this.http.put(this.baseUrl + 'private/doctor/client-request/accept/' + clientId, {}).subscribe({
      next: data => {
        console.log(data)
      },
      error: error => {

        console.error('There was an error!', error);
      }
    });

  }
}



