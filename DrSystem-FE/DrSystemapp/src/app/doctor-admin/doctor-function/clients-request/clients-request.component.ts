import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Client } from 'src/app/_models/client';
import { Doctor} from 'src/app/_models/doctor';
import { DoctorService } from 'src/app/_services/doctor.service';

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
  doctor: Doctor;
  constructor(
    private doctorService: DoctorService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    
    this.doctor = JSON.parse(localStorage.getItem("doctor"));
    console.log(this.doctor);
    this.loadDoctorClientsRequest();

  }

  loadDoctorClientsRequest() {
    this.doctorService
      .getDoctorClientsRequest()
      .subscribe((clients) => {
        this.doctor.clients = clients;
        this.filteredClients = clients;
        // sort((one, two) => (one.name < two.name ? -1 : 1));
        this.totalLength = clients.length;
      });
  }
  deleteClient(id: string) {
    console.log('Törlés');
    console.log(id);
    this.doctorService.deleteClient(id);
    for (let i = 0; i < this.doctor.clients.length; i++) {
      if (this.doctor.clients[i].id === id) {
        this.doctor.clients.splice(i, 1);
      }
    }
  }

  acceptClient(id: string) {
    console.log('Elfogad');
    console.log(id);
    this.doctorService.acceptClient(id);
    for (let i = 0; i < this.doctor.clients.length; i++) {
      if (this.doctor.clients[i].id === id) {
        this.doctor.clients.splice(i, 1);
      }
    }
  }
  Search() {
    console.log('Blablabla');

    this.filteredClients = this.doctor.clients.filter((res) => {
      return res.name.toLocaleLowerCase().match(this.name.toLocaleLowerCase());
    });
    this.totalLength = this.filteredClients.length;
  }
  key: string = 'id';
  reverse: boolean = false;
  sort(key) {
    console.log('SSZIJJAAA');
    this.key = key;
    this.reverse = !this.reverse;
  }
}
