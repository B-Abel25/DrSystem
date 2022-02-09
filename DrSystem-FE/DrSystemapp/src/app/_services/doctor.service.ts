import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Client } from '../_models/client';
import { Doctors } from '../_models/doctor';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
baseUrl= environment.apiUrl;
doctors!:Doctors[];
  constructor(private http: HttpClient) { }

  getClients(){
    return this.http.get<Client[]>(this.baseUrl+'clients');
  }
  getClient(){
    return this.http.get<Client>(this.baseUrl+'client')
  }
}
  

