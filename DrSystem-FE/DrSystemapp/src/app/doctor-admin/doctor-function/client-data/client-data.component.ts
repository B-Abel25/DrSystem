import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Appointment } from 'src/app/_models/appointment';
import { Client } from 'src/app/_models/client';
import { AppointmentService } from 'src/app/_services/appointment.service';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-client-data',
  templateUrl: './client-data.component.html',
  styleUrls: ['./client-data.component.css'],
})
export class ClientDataComponent implements OnInit {
  client: Client;
  clientAppointment: Appointment[];
  constructor(
    private doctorService: DoctorService,
    private route: ActivatedRoute,
    private appointmentService: AppointmentService
  ) {}

  ngOnInit() {
    this.loadClientData();
    this.loadClientAppointments();
  }
  loadClientData() {
    this.doctorService
      .getClientData(this.route.snapshot.paramMap.get('medNumber'))
      .subscribe((gotClient) => {
        this.client = gotClient;
        this.loadClientAppointments();
      });
  }

  loadClientAppointments() {
    this.appointmentService
      .getOneClientAppointments(this.route.snapshot.paramMap.get('medNumber'))
      .subscribe((clientOneAppointments) => {
        
        this.clientAppointment = clientOneAppointments;
       
        
      });
  }
}
