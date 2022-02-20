import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Client } from 'src/app/_models/client';
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
  clients: Client[];
  constructor(public doctorService:DoctorService, private router:Router, private toastr: ToastrService,private modalService: BsModalService, private fb:FormBuilder,private route:ActivatedRoute ) { }

  ngOnInit() {
    this.initializationForm();
    this.getCurrentDoctor();
    this.loadClients();
  }

  login(){
 
    this.doctorService.login(this.doctorLoginForm.value).subscribe(response=>{
      console.log(response)
     this.router.navigateByUrl('/admin/client-list/{{doctor.id}}');
      this.loggedIn=true;
      }, error=>{
      console.log(error);
      this.toastr.error(error.error);
     
    })
   }
   logout()
   {
     this.doctorService.logout();
     this.router.navigateByUrl('/');
    
   }
   getCurrentDoctor(){
     this.doctorService.currentDoctor$.subscribe(doctor=>{
       this.loggedIn=!!doctor;
     }, error=>{
   console.log(error);
     });
     
   }
   initializationForm(){
    this.doctorLoginForm=this.fb.group({
      sealNumber: ['', [Validators.required, Validators.pattern('[0-9]{5}')], ],
      password: ['', Validators.compose([Validators.required, Validators.minLength(8), Validators.maxLength(16)])],
    })
  }
  closeModal() {
    console.log('force close')
    if (this.modalRef != null) {
      this.modalRef.hide();
  
    }
  }
  openModal(template: TemplateRef<any>) {
    if (this.modalRef != null) {
      this.modalRef.hide();
  
    }
    this.modalRef = this.modalService.show(template);
  }
  
  loadClients(){
    this.doctorService.getClients(this.route.snapshot.paramMap.get('id')).subscribe(clients=>{
      this.clients=clients.sort((one, two) => (one.name < two.name ? -1 : 1));
    
    })
  }
}
