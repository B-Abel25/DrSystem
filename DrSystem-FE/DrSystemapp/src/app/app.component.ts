import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DrSystemapp';
  users:any;
  @Output() loggedInEvent = new EventEmitter();
 
  loggedIn: boolean = false;
  
  constructor(private http:HttpClient, private accountService:AccountService) {}
    handleLogin(state: boolean) {
      this.loggedIn = state
      this.loggedInEvent.emit(this.loggedIn);
   
     }
    
  
  ngOnInit() {
   this.setCurrentUser();
  }
  setCurrentUser(){
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
  }
 
 }


