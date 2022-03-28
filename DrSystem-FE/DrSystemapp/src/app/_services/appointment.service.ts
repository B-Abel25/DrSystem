import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Appointment } from '../_models/appointment';

@Injectable({
  providedIn: 'root',
})
export class AppointmentService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  Appointment(model: any) {
    return this.http
      .post<Appointment>(
        this.baseUrl + 'private/client/post/appointment',
        model
      )
      .pipe(
        map((response: Appointment) => {
          const client = response;
        })
      );
  }

  AppointmentDoctor(model: any) {
    return this.http
      .post<Appointment>(
        this.baseUrl + 'private/doctor/post/appointment',
        model
      )
      .pipe(
        map((response: Appointment) => {
          const doctor = response;
        })
      );
  }

  getDoctorAppointment() {
    return this.http.get<Appointment[]>(this.baseUrl + 'private/doctor/get/appointments');
  }

  getClientAppointment() {
    return this.http.get<Appointment[]>(this.baseUrl + 'private/client/get/appointments');
  }

  deleteAppointmentClient() {

    return this.http.delete(this.baseUrl + 'private/client/delete/appointment').subscribe({
      next: data => {
        console.log(data)
      },
      error: error => {

        console.error('There was an error!', error);
      }
    });

  }

  deleteAppointmentDoctor() {

    return this.http.delete(this.baseUrl + 'private/doctor/delete/appointment').subscribe({
      next: data => {
        console.log(data)
      },
      error: error => {

        console.error('There was an error!', error);
      }
    });

  }


  getOneClientAppointments(medNumber:string) {
    return this.http.get<Appointment[]>(this.baseUrl + 'private/oneClient/appointments/'+medNumber);
  }
}
