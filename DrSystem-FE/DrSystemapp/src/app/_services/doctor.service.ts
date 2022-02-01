import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Doctors } from '../_models/doctor';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
baseUrl= environment.apiUrl;
doctors!:Doctors[];
  constructor(private http: HttpClient) { }

  getDoctors(){
    return this.http.get<Doctors[]>(this.baseUrl+'user/doctors');
  }

  
}
