import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})

export class NavbarComponent implements OnInit {

  @Output() loggedInEvent = new EventEmitter();
  loggedIn: boolean = false;

  constructor() { }
  handleLogin(state: boolean) {
    this.loggedIn = state
    this.loggedInEvent.emit(this.loggedIn);
 
   }

  ngOnInit(): void {
  }

}
