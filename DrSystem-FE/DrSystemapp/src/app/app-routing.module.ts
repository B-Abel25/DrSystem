import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorLoginComponent } from './doctor-admin/doctor-login/doctor-login.component';

import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';

import { LoginComponent } from './client/login/login.component';
import { NewPasswordComponent } from './client/lost-newPassword/new-password/new-password.component';

import { RegisterComponent } from './client/register/register.component';
import { BookingComponent } from './client/clients-functions/booking/booking.component';
import { ComplaintComponent } from './client/clients-functions/complaint/complaint.component';

import { AuthGuard } from './_guards/auth.guard';
import { DrsystemHomeComponent } from './client/drsystem-home/drsystem-home.component';
import { AdminGuard } from './_guards/admin.guard';
import { ClientListComponent } from './doctor-admin/doctor-function/client-list/client-list.component';
import { DoctorMessageComponent } from './doctor-admin/doctor-function/doctor-message/doctor-message.component';
import { NewPasswordDoctorComponent } from './doctor-admin/lost-newPasswordDoctor/new-password-doctor/new-password-doctor.component';
import { DoctorMainPageComponent } from './doctor-admin/doctor-main-page/doctor-main-page.component';

const routes: Routes = [
  { path: '', component: DrsystemHomeComponent },

  { path: 'register', component: RegisterComponent },
  { path: 'new-password/:emailToken', component: NewPasswordComponent },
  { path: 'home', component: DrsystemHomeComponent },
  { path: 'login', component: LoginComponent },
  {
    path: 'admin',

    children: [
      { path: '', component: DrsystemHomeComponent },
      { path: 'login', component: DoctorLoginComponent },
      {
        path: 'new-password/:emailToken',
        component: NewPasswordDoctorComponent,
      },
    ],
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'booking', component: BookingComponent },
      { path: 'complaint', component: ComplaintComponent },
    ],
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AdminGuard],
    children: [
     
      { path: 'admin/client-list/:id', component: ClientListComponent },
    ],
  },
  

  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: NotFoundComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],

  exports: [RouterModule],
})
export class AppRoutingModule {}
