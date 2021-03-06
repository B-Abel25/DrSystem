import { HttpHeaders } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Client } from 'src/app/_models/client';
import { Doctor } from 'src/app/_models/doctor';
import { DoctorService } from 'src/app/_services/doctor.service';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-doctor-login',
  templateUrl: './doctor-login.component.html',
  styleUrls: ['./doctor-login.component.css'],
})
export class DoctorLoginComponent implements OnInit {
  loggedIn: boolean = false;
  modalRef!: BsModalRef;
  doctorLoginForm: FormGroup;
  clients: Client[];
  doctor: Doctor;
  fieldTextType: boolean;
  constructor(
    public doctorService: DoctorService,
    private router: Router,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    this.initializationForm();
    this.getCurrentDoctor();
    if (this.loggedIn) {
      this.router.navigateByUrl('/admin/doctor-page');
    }
  }

  login() {
    this.doctorService.login(this.doctorLoginForm.value).subscribe(
      (response) => {
        this.router.navigateByUrl('/admin/doctor-page');
        this.loggedIn = true;
      },
      (error) => {
        console.log(error);
        this.toastr.error(error.error);
      }
    );
  }

  logout() {
    this.doctorService.logout();
    this.router.navigateByUrl('/');
  }
  getCurrentDoctor() {
    this.doctorService.currentDoctor$.subscribe(
      (doctor) => {
        this.doctor = doctor;
        this.loggedIn = !!doctor;
      },
      (error) => {
        console.log(error);
      }
    );
  }
  initializationForm() {
    this.doctorLoginForm = this.fb.group({
      sealNumber: ['', [Validators.required, Validators.pattern('[0-9]{5}')]],
      password: [
        '',
        Validators.compose([
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(16),
        ]),
      ],
    });
  }
  closeModal() {
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
  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }
}
