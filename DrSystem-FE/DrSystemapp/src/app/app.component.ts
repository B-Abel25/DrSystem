import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Doctors } from './_models/doctor';
import { DoctorAdmin } from './_models/doctorsadmin';
import { Registration } from './_models/registration';
import { AccountService } from './_services/account.service';
import { DoctorService } from './_services/doctor.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DrSystemapp';
  users:any;
  @Output() loggedInEvent = new EventEmitter();
 
  loggedIn: boolean = false;
  
  constructor(private http:HttpClient, private accountService:AccountService, private doctorService:DoctorService) {}
    handleLogin(state: boolean) {
      this.loggedIn = state
      this.loggedInEvent.emit(this.loggedIn);
   
     }
    
  
  ngOnInit() {
   this.setCurrentClient();
  }
  setCurrentClient(){
    const client: Registration = JSON.parse(localStorage.getItem('client'));
    this.accountService.setCurrentClient(client);
  }
  setCurrentDoctor(){
    const doctor: DoctorAdmin = JSON.parse(localStorage.getItem('doctor'));
    this.doctorService.setCurrentDoctor(doctor);
  }
 
 }


