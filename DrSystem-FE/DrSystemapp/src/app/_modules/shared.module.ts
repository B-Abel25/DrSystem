import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ReactiveFormsModule } from '@angular/forms';
import {NgxPaginationModule} from 'ngx-pagination';


@NgModule({
  declarations: [],
  imports: [
    BsDatepickerModule.forRoot(),
    CommonModule,
    ModalModule.forRoot(),
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
      
    }),
    ReactiveFormsModule,
    NgxPaginationModule,


  ],
  exports: [
    BsDropdownModule,
    ToastrModule,
    ModalModule,
    BsDatepickerModule,
    ReactiveFormsModule,
    NgxPaginationModule,
  ]
})
export class SharedModule { }
