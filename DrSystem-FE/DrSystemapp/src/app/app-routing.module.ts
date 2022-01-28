import { registerLocaleData } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomepageComponent } from './homepage/homepage.component';

import { LoginComponent } from './login/login.component';
import { NewPasswordComponent } from './new-password/new-password.component';


import { RegisterComponent } from './register/register.component';
import { BookingComponent } from './users/booking/booking.component';
import { ComplaintComponent } from './users/complaint/complaint.component';

import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
 {path: 'new-password/:id', component: NewPasswordComponent},
  {path: '', component: LoginComponent},
  
  {
    path:'',
    runGuardsAndResolvers:'always',
    canActivate: [AuthGuard],
    children:[
      {path: 'booking', component: BookingComponent},
      {path: 'complaint', component: ComplaintComponent},
      
    ]
  },
  {path: '**', component: LoginComponent, pathMatch: 'full'},
 
 
  
 

    
  
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  

  exports: [RouterModule]
})
export class AppRoutingModule { }
