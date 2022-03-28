import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Client } from 'src/app/_models/client';
import { Doctor } from 'src/app/_models/doctor';
import { DoctorService } from 'src/app/_services/doctor.service';
import jwt_decode from 'jwt-decode';
import { HttpHeaders } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-clients-request',
  templateUrl: './clients-request.component.html',
  styleUrls: ['./clients-request.component.css'],
})
export class ClientsRequestComponent implements OnInit {
  filteredClients: Client[];
  totalLength: any;
  page: number = 1;
  name: any;
  filterTerm: string;
  doctor: Doctor;
  constructor(
    private doctorService: DoctorService,

    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.doctor = JSON.parse(localStorage.getItem('doctor'));
    let contentHeader = new HttpHeaders({ 'Content-Type': 'application/json' });
    console.log(contentHeader);
    //const tokenInfo = this.getDecodedAccessToken(); // decode token
    //const expireDate = tokenInfo.exp; // get token expiration dateTime
    //console.log(tokenInfo);
    this.loadDoctorClientsRequest();
  }

  loadDoctorClientsRequest() {
    this.doctorService.getDoctorClientsRequest().subscribe((clients) => {
      this.doctor.clients = clients;
      this.filteredClients = clients;
      // sort((one, two) => (one.name < two.name ? -1 : 1));
      this.totalLength = clients.length;
    });
  }
  deleteClient(medNumber: string) {
    this.doctorService.deleteClient(medNumber);
    for (let i = 0; i < this.doctor.clients.length; i++) {
      if (this.doctor.clients[i].medNumber === medNumber) {
        this.doctor.clients.splice(i, 1);
      }
    }
    this.toastr.error('Sikeresen elutasította a kérelmet!');
  }

  acceptClient(medNumber: string) {
    this.doctorService.acceptClient(medNumber);
    for (let i = 0; i < this.doctor.clients.length; i++) {
      if (this.doctor.clients[i].medNumber === medNumber) {
        this.doctor.clients.splice(i, 1);
      }
    }
    this.toastr.success('Sikeresen elfogadta a kérelmet!');
  }
  Search() {
    this.filteredClients = this.doctor.clients.filter((res) => {
      return res.name.toLocaleLowerCase().match(this.name.toLocaleLowerCase());
    });
    this.totalLength = this.filteredClients.length;
  }
  key: string = 'id';
  reverse: boolean = false;
  sort(key) {
    this.key = key;
    this.reverse = !this.reverse;
  }
  getDecodedAccessToken(token: string): any {
    try {
      return jwt_decode(token);
    } catch (Error) {
      return null;
    }
  }
}
