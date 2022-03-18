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
}
