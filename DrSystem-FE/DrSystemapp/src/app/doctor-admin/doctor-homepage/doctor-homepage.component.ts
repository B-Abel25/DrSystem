import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Doctors } from 'src/app/_models/doctor';
import { DoctorAdmin } from 'src/app/_models/doctorsadmin';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-doctor-homepage',
  templateUrl: './doctor-homepage.component.html',
  styleUrls: ['./doctor-homepage.component.css']
})
export class DoctorHomepageComponent implements OnInit {

  constructor(private http:HttpClient, private doctorService:DoctorService) { }

  ngOnInit() {
    this.setCurrentDoctor();
   }
   setCurrentDoctor(){
     const doctor: DoctorAdmin = JSON.parse(localStorage.getItem('doctor'));
     this.doctorService.setCurrentUser(doctor);
   }

}
