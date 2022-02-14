import { Component, OnInit } from '@angular/core';
import { Registration } from '../../_models/registration';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css']
})
export class HomepageComponent implements OnInit {
 
 
  constructor(private accountService:AccountService) { }

  ngOnInit() {
    this.setCurrentClient();
  }
  setCurrentClient(){
    const client: Registration = JSON.parse(localStorage.getItem('client'));
    this.accountService.setCurrentClient(client);
  }
  

}
