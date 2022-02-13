import { registerLocaleData } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorLoginComponent } from './doctor-admin/doctor-login/doctor-login.component';

import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { HomepageComponent } from './client/homepage/homepage.component';

import { LoginComponent } from './client/login/login.component';
import { NewPasswordComponent } from './client/lost-newPassword/new-password/new-password.component';


import { RegisterComponent } from './client/register/register.component';
import { BookingComponent } from './client/clients-functions/booking/booking.component';
import { ComplaintComponent } from './client/clients-functions/complaint/complaint.component';

import { AuthGuard } from './_guards/auth.guard';
import { DrsystemHomeComponent } from './client/drsystem-home/drsystem-home.component';

const routes: Routes = [
  
  {path: '', component: DrsystemHomeComponent},
  {
    path:'drsystem',
    component:HomepageComponent,
      children:[
      {path: 'register', component: RegisterComponent},
      {path: 'new-password/:id', component: NewPasswordComponent},
      {path: 'home', component:DrsystemHomeComponent},
       {path: 'login', component: LoginComponent},
    ]
  },
  {
    path:'',
    runGuardsAndResolvers:'always',
    canActivate: [AuthGuard],
    children:[
      {path: 'booking', component: BookingComponent},
      {path: 'complaint', component: ComplaintComponent},
      
    ]
  },
  {path: 'admin', component: DoctorLoginComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent, pathMatch:'full'},
  
 
 
  
 

    
  
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  

  exports: [RouterModule]
})
export class AppRoutingModule { }
