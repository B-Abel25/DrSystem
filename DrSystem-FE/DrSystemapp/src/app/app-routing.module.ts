import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { LostpasswordComponent } from './lostpassword/lostpassword.component';
import { NewpasswordComponent } from './newpassword/newpassword.component';
import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  {path:'',redirectTo:'login',pathMatch:'full'},
  { path: 'new-password/:token', component: NewpasswordComponent },
  { path: 'lost-password', component: LostpasswordComponent},
  { path: 'register', component:RegisterComponent },
  { path: 'login', component: LoginComponent }
    
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
