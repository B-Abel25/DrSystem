import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Client } from 'src/app/_models/client';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-clients-request',
  templateUrl: './clients-request.component.html',
  styleUrls: ['./clients-request.component.css'],
})
export class ClientsRequestComponent implements OnInit {
  clients: Client[];
  filteredClients: Client[];
  totalLength: any;
  page: number = 1;
  name: any;
  constructor(
    private doctorService: DoctorService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.loadDoctorClientsRequest();
  }

  loadDoctorClientsRequest() {
    this.doctorService
      .getDoctorClientsRequest(this.route.snapshot.paramMap.get('id'))
      .subscribe((clients) => {
        this.clients = clients;
        this.filteredClients = clients;
        // sort((one, two) => (one.name < two.name ? -1 : 1));
        this.totalLength = clients.length;
        console.log(this.clients);
      });
  }
  deleteClient(id: string) {
    console.log('Törlés');
    console.log(id);
    this.doctorService.deleteClient(id);
    for (let i = 0; i < this.clients.length; i++) {
      if (this.clients[i].id === id) {
        this.clients.splice(i, 1);
      }
    }
  }

  acceptClient(id: string) {
    console.log('Elfogad');
    console.log(id);
    this.doctorService.acceptClient(id);
    for (let i = 0; i < this.clients.length; i++) {
      if (this.clients[i].id === id) {
        this.clients.splice(i, 1);
      }
    }
  }
  Search() {
    console.log('Blablabla');

    this.filteredClients = this.clients.filter((res) => {
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
