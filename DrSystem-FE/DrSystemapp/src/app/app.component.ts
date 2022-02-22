import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';

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
  users: any;
  @Output() loggedInEvent = new EventEmitter();

  loggedIn: boolean = false;

  constructor(private http: HttpClient, public accountService: AccountService, private doctorService: DoctorService) { }
  handleLogin(state: boolean) {
    this.loggedIn = state
    this.loggedInEvent.emit(this.loggedIn);

  }


  ngOnInit() {
    this.handleLogin(this.loggedIn);
    this.setCurrentClient();
    this.setCurrentDoctor();
    this.getCurrentClient();
    this.getCurrentDoctor();
  }
  setCurrentClient() {
    const client: Registration = JSON.parse(localStorage.getItem('client'));
    this.accountService.setCurrentClient(client);
  }
  setCurrentDoctor() {
    const doctor: DoctorAdmin = JSON.parse(localStorage.getItem('doctor'));
    this.doctorService.setCurrentDoctor(doctor);
  }
  getCurrentClient() {
    this.accountService.currentClient$.subscribe(client => {
      this.loggedIn = !!client;
    }, error => {
      console.log(error);
    });

  }
  getCurrentDoctor() {
    this.accountService.currentClient$.subscribe(doctor => {
      this.loggedIn = !!doctor;
    }, error => {
      console.log(error);
    });

  }

}


