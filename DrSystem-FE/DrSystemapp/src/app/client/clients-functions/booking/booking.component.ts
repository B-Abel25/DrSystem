import { LocationStrategy } from '@angular/common';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css']
})
export class BookingComponent implements OnInit {

  constructor(private location: LocationStrategy) { 
    history.pushState(null, null, window.location.href);  
this.location.onPopState(() => {
  history.pushState(null, null, window.location.href);
});  
  }

  ngOnInit(): void {
  }

}
