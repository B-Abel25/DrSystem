import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Doctor } from 'src/app/_models/doctor';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-admin-navbar',
  templateUrl: './admin-navbar.component.html',
  styleUrls: ['./admin-navbar.component.css'],
})
export class AdminNavbarComponent implements OnInit {
  constructor(public doctorService: DoctorService, private router: Router, private route: ActivatedRoute) {}
  loggedIn: boolean = false;
  doctor:Doctor;
  ngOnInit() {
    this.loadClients();
  }
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

  loadClients(){
    
    this.doctorService.getClients(this.route.snapshot.paramMap.get('id')).subscribe(doctor=>{
      this.doctor=doctor;
      // sort((one, two) => (one.name < two.name ? -1 : 1));
    
    })
  }

  
}
