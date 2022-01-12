import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLoginComponent } from './admin-login/admin-login.component';
import { LoginComponent } from './login/login.component';
import { LostpasswordComponent } from './lostpassword/lostpassword.component';
import { NewpasswordComponent } from './newpassword/newpassword.component';
import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  {path:'',redirectTo:'login',pathMatch:'full'},
  { path: 'new-password/:token', component: NewpasswordComponent },
  { path: 'lost-password', component: LostpasswordComponent},
  { path: 'register', component:RegisterComponent },
  { path: 'login', component: LoginComponent },
  {path: 'admin', component:AdminLoginComponent, outlet:'admin'}
    
  
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  

  exports: [RouterModule]
})
export class AppRoutingModule { }
