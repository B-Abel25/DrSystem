import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-doctor-login',
  templateUrl: './doctor-login.component.html',
  styleUrls: ['./doctor-login.component.css']
})
export class DoctorLoginComponent implements OnInit {

  loggedIn: boolean = false;
  modalRef!: BsModalRef;
  doctorLoginForm:FormGroup;
  constructor(public doctorService:DoctorService, private router:Router, private toastr: ToastrService,private modalService: BsModalService, private fb:FormBuilder ) { }

  ngOnInit() {
    this.initializationForm();
    this.getCurrentUser();
  }

  login(){
 
    this.doctorService.login(this.doctorLoginForm.value).subscribe(response=>{
     this.router.navigateByUrl('/client-list');
      this.loggedIn=true;
      }, error=>{
      console.log(error);
     
    })
   }
   logout()
   {
     this.doctorService.logout();
     this.router.navigateByUrl('/');
    
   }
   getCurrentUser(){
     this.doctorService.currentDoctor$.subscribe(user=>{
       this.loggedIn=!!user;
     }, error=>{
   console.log(error);
     });
     
   }
   initializationForm(){
    this.doctorLoginForm=this.fb.group({
      doctorNumber: ['', [Validators.required, Validators.pattern('O[0-9]{5}')], ],
      password: ['', Validators.compose([Validators.required, Validators.minLength(8), Validators.maxLength(16)])],
    })
  }
}
