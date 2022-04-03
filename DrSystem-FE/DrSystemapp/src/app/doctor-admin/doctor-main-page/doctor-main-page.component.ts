import { LocationStrategy } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Client } from 'src/app/_models/client';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-doctor-main-page',
  templateUrl: './doctor-main-page.component.html',
  styleUrls: ['./doctor-main-page.component.css']
})
export class DoctorMainPageComponent implements OnInit {

  constructor(private location: LocationStrategy, private messageService: MessageService,
    private route: ActivatedRoute) {
    history.pushState(null, null, window.location.href);
    this.location.onPopState(() => {
      history.pushState(null, null, window.location.href);
    });
  }
  clients: Client[];
  filteredClients: Client[];
  
 
  filterTerm: string;
  totalLength: any;
  page: number = 1;
  name: any;
  ngOnInit(): void {
    this.loadClients();
  }
  loadClients() {
    this.messageService.getDoctorUnreadMessages().subscribe((response) => {
      this.clients = response;
      this.filteredClients = response;
      // sort((one, two) => (one.name < two.name ? -1 : 1));
      this.totalLength = response.length;
    });
  }

  Search() {
    this.filteredClients = this.clients.filter((res) => {
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
  

}
