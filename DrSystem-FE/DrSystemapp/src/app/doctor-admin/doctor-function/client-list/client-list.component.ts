import {
  Component,
  Directive,
  EventEmitter,
  Input,
  OnInit,
  Output,
  PipeTransform,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Client } from 'src/app/_models/client';

import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css'],
})
export class ClientListComponent implements OnInit {
  clients: Client[];
  filteredClients: Client[];
  constructor(
    private doctorService: DoctorService,
    private route: ActivatedRoute
  ) {}
  filterTerm:string;
  totalLength: any;
  page: number = 1;
  name: any;
  ngOnInit() {
    this.loadDoctorClients();
  }

  loadDoctorClients() {
    this.doctorService
      .getDoctorClients()
      .subscribe((clients) => {
        this.filteredClients = clients;
        this.clients = clients;
        // sort((one, two) => (one.name < two.name ? -1 : 1));
        this.totalLength = clients.length;
        localStorage.setItem('clients', JSON.stringify(this.clients));
      });
  }

  Search() {
    console.log('Blablabla');
    
      this.filteredClients = this.clients.filter((res) => {
        return res.name
          .toLocaleLowerCase()
          .match(this.name.toLocaleLowerCase());
      });
      this.totalLength = this.filteredClients.length;
    
  }
  key: string = 'id';
  reverse: boolean = false;
  sort(key) {
    this.key = key;
    this.reverse = !this.reverse;
  }
}
