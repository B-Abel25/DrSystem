import { HttpClient } from '@angular/common/http';
import { identifierModuleUrl } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

import { map, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Client } from '../_models/client';
import { Doctor } from '../_models/doctor';

import { LostPassword } from '../_models/lostpasswordrequest';
import { NewPassword } from '../_models/newpassword';

@Injectable({
  providedIn: 'root',
})
export class DoctorService {
  baseUrl = environment.apiUrl;
  //doctors!: DoctorAdmin[];
  private currentDoctorSource = new ReplaySubject<Doctor>(1);
  currentDoctor$ = this.currentDoctorSource.asObservable();
  singleuserdata: Client[];
  constructor(private http: HttpClient, private toastr: ToastrService) {}

  login(model: any) {
    return this.http
      .put<Doctor>(this.baseUrl + 'public/doctor/login', model)
      .pipe(
        map((response: Doctor) => {
          const doctor = response;
          if (doctor) {
            this.setCurrentDoctor(doctor);
            localStorage.setItem('doctor', JSON.stringify(doctor));
          }
          this.toastr.success('Belépés sikeres!');
        })
      );
  }
  setCurrentDoctor(doctor: Doctor) {
    localStorage.setItem('doctor', JSON.stringify(doctor));
    this.currentDoctorSource.next(doctor);
  }

  logout() {
    localStorage.clear();
    this.currentDoctorSource.next(null as any);
  }
  lostPassword(model: any) {
    return this.http
      .put<LostPassword>(this.baseUrl + 'public/lost-password', model)
      .pipe(
        map((password: LostPassword) => {
          if (password) {
            localStorage.setItem('password', JSON.stringify(password));
            console.log(model);
          }
        })
      );
  }

  newPassword(model: any) {
    return this.http
      .post<NewPassword>(this.baseUrl + 'public/new-password', model)
      .pipe(
        map((password: NewPassword) => {
          if (password) {
            console.log(model);
          }
        })
      );
  }
  getDoctorClientsRequest() {
    return this.http.get<Client[]>(
      this.baseUrl + 'private/doctor/clients-request'
    );
  }
  getDoctorClients() {
    return this.http.get<Client[]>(this.baseUrl + 'private/doctor/clients');
  }

  deleteClient(medNumber: string) {
    return this.http
      .delete(
        this.baseUrl + 'private/doctor/client-request/decline/' + medNumber
      )
      .subscribe({
        next: (data) => {
          console.log(data);
        },
        error: (error) => {
          console.error('There was an error!', error);
        },
      });
  }
  acceptClient(medNumber: string) {
    return this.http
      .put(
        this.baseUrl + 'private/doctor/client-request/accept/' + medNumber,
        {}
      )
      .subscribe({
        next: (data) => {
          console.log(data);
        },
        error: (error) => {
          console.error('There was an error!', error);
        },
      });
  }
  getDoctors() {
    return this.http.get<Doctor[]>(this.baseUrl + 'public/register/doctors');
  }

  getClientData(medNumber: string) {
    return this.http.get<Client>(
      this.baseUrl + 'private/doctor/get/client/' + medNumber
    );
  }
}
