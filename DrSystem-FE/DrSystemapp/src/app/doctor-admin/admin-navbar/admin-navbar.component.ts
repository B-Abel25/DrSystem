import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-admin-navbar',
  templateUrl: './admin-navbar.component.html',
  styleUrls: ['./admin-navbar.component.css'],
})
export class AdminNavbarComponent implements OnInit {
  constructor(public doctorService: DoctorService, private router: Router) {}
  loggedIn: boolean = false;
  ngOnInit(): void {}
  logout() {
    this.doctorService.logout();
    this.router.navigateByUrl('/admin/login');
  }
  getCurrentDoctor() {
    this.doctorService.currentDoctor$.subscribe(
      (doctor) => {
        this.loggedIn = !!doctor;
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
