

import { Component, Directive, EventEmitter, Input, OnInit, Output, PipeTransform, QueryList, ViewChildren } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Client } from 'src/app/_models/client';

import { DoctorService } from 'src/app/_services/doctor.service';

 const CLIENT:Client[]=[];

function search(text: string, pipe: PipeTransform): Client[] {
  return CLIENT.filter(client => {
    const term = text.toLowerCase();
    return client.name.toLowerCase().includes(term)
        || pipe.transform(client.place).includes(term)
        || pipe.transform(client.medNumber).includes(term);
  });
}

export type SortColumn = keyof Client | '';
export type SortDirection = 'asc' | 'desc' | '';
const rotate: {[key: string]: SortDirection} = { 'asc': 'desc', 'desc': '', '': 'asc' };

const compare = (v1: string | number, v2: string | number) => v1 < v2 ? -1 : v1 > v2 ? 1 : 0;

export interface SortEvent {
  column: SortColumn;
  direction: SortDirection;
}

@Directive({
  selector: 'th[sortable]',
  host: {
    '[class.asc]': 'direction === "asc"',
    '[class.desc]': 'direction === "desc"',
    '(click)': 'rotate()'
  }
})

export class NgbdSortableHeader {

  @Input() sortable: SortColumn = '';
  @Input() direction: SortDirection = '';
  @Output() sort = new EventEmitter<SortEvent>();

  rotate() {
    this.direction = rotate[this.direction];
    this.sort.emit({column: this.sortable, direction: this.direction});
  }
}
@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css'],
})
export class ClientListComponent implements OnInit {
 
  clients=CLIENT;
 
  clients$:Observable<Client[]>;
  filter = new FormControl('');
  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader>;
  constructor(private doctorService: DoctorService, private route: ActivatedRoute) {
    
  
  }

   

  ngOnInit() {
    this.loadDoctorClients();
  }

  loadDoctorClients() {

    this.doctorService.getDoctorClients(this.route.snapshot.paramMap.get('id')).subscribe(clients => {
      this.clients = clients;
      // sort((one, two) => (one.name < two.name ? -1 : 1));

    })
  }
 
  onSort({column, direction}: SortEvent) {

    console.log('OKKKKK');
    console.log(column);
    console.log(direction);
    this.headers.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    
    if (direction === '' || column === '') {
      this.clients = CLIENT;
    } else {
      this.clients = [...CLIENT].sort((a, b) => {
        const res = compare(a[column], b[column]);
        return direction === 'asc' ? res : -res;
      });
    }
  }
 
}


