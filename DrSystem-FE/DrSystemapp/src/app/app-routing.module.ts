import { registerLocaleData } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { HomepageComponent } from './homepage/homepage.component';

import { LoginComponent } from './login/login.component';

import { RegisterComponent } from './register/register.component';

import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [

  {path: '', component: HomepageComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  
  {
    path:'',
    runGuardsAndResolvers:'always',
    canActivate: [AuthGuard],
    children:[
      
    ]
  },

  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent, pathMatch:'full'},
 
 
 
  
 

    
  
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  

  exports: [RouterModule]
})
export class AppRoutingModule { }
