import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClientMessagesComponent } from 'src/app/client/clients-functions/client-messages/client-messages.component';
import { Client } from 'src/app/_models/client';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-client-data',
  templateUrl: './client-data.component.html',
  styleUrls: ['./client-data.component.css']
})
export class ClientDataComponent implements OnInit {
client:Client;
  constructor(private doctorService:DoctorService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadMember();
    console.log(this.client);

    
    
  }
loadMember(){  
  
    this.client = JSON.parse(localStorage.getItem("clients")).find(item=>item.medNumber===this.route.snapshot.paramMap.get('medNumber'));
}
}
