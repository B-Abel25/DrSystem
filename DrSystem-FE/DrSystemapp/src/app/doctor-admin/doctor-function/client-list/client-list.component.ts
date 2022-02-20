import { Component, OnInit } from '@angular/core';
import { Client } from 'src/app/_models/client';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css'],
})
export class ClientListComponent implements OnInit {
  clients:Client[];
  constructor(private doctorService:DoctorService) {}

  ngOnInit() {
    this.loadClients();
  }

  loadClients(){
    this.doctorService.getClients().subscribe(clients=>{
      this.clients=clients.sort((one, two) => (one.name < two.name ? -1 : 1));
    
    })
  }
}
