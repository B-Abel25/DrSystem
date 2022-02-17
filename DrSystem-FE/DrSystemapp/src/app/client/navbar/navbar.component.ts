import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})

export class NavbarComponent implements OnInit {

 
  public model: any={}
  loggedIn: boolean = false;
 
  constructor(public accountService:AccountService, private router:Router, private toastr: ToastrService ) { }

  ngOnInit(): void {
   
    this.getCurrentClient();
  }
login(){
 this.accountService.login(this.model).subscribe(response=>{
  this.router.navigateByUrl('/booking');
   this.loggedIn=true;
   }, error=>{
   console.log(error);
   this.toastr.error(error.error);
 })
}
logout()
{
  this.accountService.logout();
  this.router.navigateByUrl('/login');
 
}
getCurrentClient(){
  this.accountService.currentClient$.subscribe(client=>{
    this.loggedIn=!!client;
  }, error=>{
console.log(error);
  });
  
}

}
