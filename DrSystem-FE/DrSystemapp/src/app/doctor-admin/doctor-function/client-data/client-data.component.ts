import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Appointment } from 'src/app/_models/appointment';
import { Client } from 'src/app/_models/client';
import { AppointmentService } from 'src/app/_services/appointment.service';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-client-data',
  templateUrl: './client-data.component.html',
  styleUrls: ['./client-data.component.css']
})
export class ClientDataComponent implements OnInit {
client:Client;
clientAppointment:Appointment[];
  constructor(private doctorService:DoctorService, private route: ActivatedRoute, private appointmentService:AppointmentService) { }

  ngOnInit(): void {
    this.loadClientData();
    this.loadClientApointments();

    
    
  }
loadClientData(){  
  
    this.client = JSON.parse(localStorage.getItem("clients"))
    .find(item=>item.medNumber===this.route.snapshot.paramMap.get('medNumber'));
}

loadClientApointments(){  
  this.client = JSON.parse(localStorage.getItem("clients"))
    .find(item=>item.medNumber===this.route.snapshot.paramMap.get('medNumber'));
 this.appointmentService.getOneClientAppointments(this.client.medNumber).subscribe(clientOneAppointments =>{
this.clientAppointment=clientOneAppointments;
 }) 
 
}
}
