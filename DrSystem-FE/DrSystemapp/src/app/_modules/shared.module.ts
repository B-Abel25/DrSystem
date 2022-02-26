import { Component, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ReactiveFormsModule } from '@angular/forms';
import {NgxPaginationModule} from 'ngx-pagination';
import {TabsModule} from 'ngx-bootstrap/tabs';
import { MbscModule, MbscProvider } from "ack-angular-mobiscroll"



@Component({
  selector: 'my-app',
  template: '<input type="datetime" mbsc-calendar />'
}) export class AppComponent {}

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
    TabsModule.forRoot(),
    
   

  ],
  exports: [
    BsDropdownModule,
    ToastrModule,
    ModalModule,
    BsDatepickerModule,
    ReactiveFormsModule,
    NgxPaginationModule,
    TabsModule,
    
    
    
   
    
  ]
})
export class SharedModule { }
