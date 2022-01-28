import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from "@angular/common/http";
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { NavbarComponent } from './navbar/navbar.component';
import { HomepageComponent } from './homepage/homepage.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';



import{ToastrModule} from 'ngx-toastr';

import { LostPasswordRequestComponent } from './lostpassword/lost-password-request/lost-password-request.component';
import { BookingComponent } from './users/booking/booking.component';
import { ComplaintComponent } from './users/complaint/complaint.component';
import { NewPasswordComponent } from './new-password/new-password.component';

@NgModule({
  declarations: [
   
    AppComponent,
    LoginComponent,
    RegisterComponent,
    NavbarComponent,
    HomepageComponent,
    
    LostPasswordRequestComponent,
          BookingComponent,
          ComplaintComponent,
          NewPasswordComponent,
    
  
  
    
  ],
  imports: [
    ModalModule.forRoot(),FormsModule,
    BrowserModule, HttpClientModule,
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(),
    AppRoutingModule,
    ToastrModule.forRoot({
      positionClass:'toast-bottom-right'
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
