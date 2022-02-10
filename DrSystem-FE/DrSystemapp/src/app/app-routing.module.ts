import { registerLocaleData } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorLoginComponent } from './doctor-login/doctor-login.component';

import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { HomepageComponent } from './homepage/homepage.component';

import { LoginComponent } from './login/login.component';
import { NewPasswordComponent } from './lost-newpassword/new-password/new-password.component';


import { RegisterComponent } from './register/register.component';
import { BookingComponent } from './users/booking/booking.component';
import { ComplaintComponent } from './users/complaint/complaint.component';

import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  
  {path: '', component: HomepageComponent},
  {
    path:'drsystem',
    component:HomepageComponent,
      children:[
      {path: 'register', component: RegisterComponent},
      {path: 'new-password/:id', component: NewPasswordComponent},
      
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
