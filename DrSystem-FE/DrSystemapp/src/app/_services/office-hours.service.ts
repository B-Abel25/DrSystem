import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { officeHours } from '../_models/officeHours';

@Injectable({
  providedIn: 'root'
})
export class OfficeHoursService {
  baseUrl = environment.apiUrl;
  constructor(private http:HttpClient) { }
  
  officeHours(model: any) {
    return this.http
      .post<officeHours>(
        this.baseUrl + 'private/doctor/office-hours',
        model
      )
      .pipe(
        map((response: officeHours) => {
          const officeHours = response;
        })
      );
  }

  Duration(model: any) {
    return this.http
      .post<officeHours>(
        this.baseUrl + 'private/doctor/get/duration',
        model
      )
      .pipe(
        map((response: officeHours) => {
          const duration = response;
        })
      );
  }

  officeHoursPut(modal:any) {


    return this.http.put(this.baseUrl + 'private/doctor/office-hours/modify',modal).subscribe({
      next: data => {
        console.log(data)
      },
      error: error => {

        console.error('There was an error!', error);
      }
    });

  }
  
  durationPut(duration:any) {


    return this.http.put(this.baseUrl + '​private​/doctor​/put​/duration​/'+duration, {}).subscribe({
      next: data => {
        console.log(data)
      },
      error: error => {

        console.error('There was an error!', error);
      }
    });

  }

}
