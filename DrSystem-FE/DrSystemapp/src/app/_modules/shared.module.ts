import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule, ToastrService } from 'ngx-toastr';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ModalModule.forRoot(),
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass:'toast-bottom-right'
    }),
    
  ],
  exports:[
    BsDropdownModule,
    ToastrModule,
    ModalModule,
  ]
})
export class SharedModule { }
