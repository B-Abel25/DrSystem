import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
 
  flag:boolean = true
  title = 'Angular Application';
  isSideBarOpen = true;

  

  apply(value:string){
    this.flag = value == "login"?true : false;
  }
 
}