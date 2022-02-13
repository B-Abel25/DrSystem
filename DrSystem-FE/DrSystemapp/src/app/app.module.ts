import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './client/login/login.component';
import { RegisterComponent } from './client/register/register.component';
import { NavbarComponent } from './client/navbar/navbar.component';
import { HomepageComponent } from './client/homepage/homepage.component';
import { DoctorLoginComponent } from './doctor-admin/doctor-login/doctor-login.component';





import { LostPasswordRequestComponent } from './client/lost-newPassword/lost-password-request/lost-password-request.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { SharedModule } from './_modules/shared.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { ClientHomepageComponent } from './client/client-homepage/client-homepage.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { AdminNavbarComponent } from './doctor-admin/admin-navbar/admin-navbar.component';
import { DrsystemHomeComponent } from './client/drsystem-home/drsystem-home.component';




@NgModule({
  declarations: [
   
    AppComponent,
    LoginComponent,
    RegisterComponent,
    NavbarComponent,
    HomepageComponent,
    
    LostPasswordRequestComponent,
          NotFoundComponent,
          ServerErrorComponent,
          TextInputComponent,
         DoctorLoginComponent,
         ClientHomepageComponent,
         AdminNavbarComponent,
         DrsystemHomeComponent,
        
    
  
  
    
  ],
  imports: [
    FormsModule,
    BrowserModule, 
    HttpClientModule,
    BrowserAnimationsModule,
   NgxSpinnerModule,
    AppRoutingModule,
    SharedModule,
    ReactiveFormsModule,
   

    
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
