import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DrSystemapp';
  users:any;
  @Output() loggedInEvent = new EventEmitter();
  @Output() adminloggedInEvent= new EventEmitter();
  loggedIn: boolean = false;
  adminloggedIn: boolean=false;
  constructor(private http:HttpClient) {}
    handleLogin(state: boolean) {
      this.loggedIn = state
      this.loggedInEvent.emit(this.loggedIn);
   
     }
    
  
  ngOnInit() {
   
  }
 
 }


