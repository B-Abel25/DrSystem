import { LocationStrategy } from '@angular/common';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-doctor-main-page',
  templateUrl: './doctor-main-page.component.html',
  styleUrls: ['./doctor-main-page.component.css']
})
export class DoctorMainPageComponent implements OnInit {

  constructor(private location: LocationStrategy) {
    history.pushState(null, null, window.location.href);
    this.location.onPopState(() => {
      history.pushState(null, null, window.location.href);
    });
  }

  ngOnInit() {
  }

}
