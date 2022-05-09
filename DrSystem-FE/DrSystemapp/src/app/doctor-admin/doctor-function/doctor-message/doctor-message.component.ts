import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Client } from 'src/app/_models/client';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-doctor-message',
  templateUrl: './doctor-message.component.html',
  styleUrls: ['./doctor-message.component.css'],
})
export class DoctorMessageComponent implements OnInit {
  clients: Client[];
  filteredClients: Client[];
  
  constructor(
    private messageService: MessageService,
    private route: ActivatedRoute
  ) {}
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
