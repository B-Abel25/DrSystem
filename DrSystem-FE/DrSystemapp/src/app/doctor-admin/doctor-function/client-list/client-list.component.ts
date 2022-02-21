import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Client } from 'src/app/_models/client';
import { Doctor } from 'src/app/_models/doctor';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css'],
})
export class ClientListComponent implements OnInit {
  clients:Client[];
  client:Doctor;
  constructor(private doctorService:DoctorService, private route: ActivatedRoute) {}

  ngOnInit() {
    this.loadDoctorClients();
  }

  loadDoctorClients(){
    
    this.doctorService.getDoctorClients(this.route.snapshot.paramMap.get('id')).subscribe(client=>{
      this.client=client;
      // sort((one, two) => (one.name < two.name ? -1 : 1));
    
    })
  }
}
