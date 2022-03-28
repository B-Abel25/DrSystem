import {
  Component,
  Directive,
  EventEmitter,
  Input,
  OnInit,
  Output,
  PipeTransform,
  QueryList,
  TemplateRef,
  ViewChildren,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
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
  modalRef!: BsModalRef;
  constructor(
    private doctorService: DoctorService,
    private route: ActivatedRoute,
    private modalService: BsModalService
  ) {}
  filterTerm: string;
  totalLength: any;
  page: number = 1;
  name: any;
  ngOnInit() {
    this.loadDoctorClients();
    console.log('HALIHOO');
  }

  loadDoctorClients() {
    this.doctorService.getDoctorClients().subscribe((clients) => {
      this.filteredClients = clients;
      this.clients = clients;
      // sort((one, two) => (one.name < two.name ? -1 : 1));
      this.totalLength = clients.length;
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
  openModal(template: TemplateRef<any>) {
    if (this.modalRef != null) {
      this.modalRef.hide();
    }
    this.modalRef = this.modalService.show(template);
  }
  closeModal() {
    if (this.modalRef != null) {
      this.modalRef.hide();
    }
  }
}
