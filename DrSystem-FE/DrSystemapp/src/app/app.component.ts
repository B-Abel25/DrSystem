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
     adminhandleLogin(state: boolean) {
      this.adminloggedIn = state
      this.adminloggedInEvent.emit(this.adminloggedIn);
   
     }
  
  ngOnInit() {
    this.getUsers();
  }
  getUsers()
{
  this.http.get('https://localhost:5001/WeatherForecast').subscribe(response=>{
    this.users=response;
    console.log(response);
  },error=>{
    console.log(error);
  })
}}


