import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Doctors } from '../_models/doctor';
import { AccountService } from '../_services/account.service';
import { DoctorService } from '../_services/doctor.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  
  @Output() cancelRegister= new EventEmitter();
model: any={}
  registerForm!: FormGroup;
  doctors!:Doctors[];
  constructor(private accountService:AccountService, private toatsr:ToastrService, private doctorService:DoctorService) { }

  ngOnInit(): void {
    this.intitializeForm();
    this.loadDoctors();
  }

  intitializeForm()
  {
    this.registerForm= new FormGroup({
      name: new FormControl('',Validators.required),
      password: new FormControl('',[Validators.required, Validators.minLength(6), Validators.maxLength(12)]),
      confirmPassword: new FormControl('', Validators.required),
    })
  }
  // matchValue(matchTo: string  ): ValidatorFn{
  //   return (control: AbstractControl) => {
  //     return control?.value === control?.parent?.controls[matchTo].value
  //     ? null : {isMatching: true}
  //   }
  // }

register(){
  this.accountService.register(this.model).subscribe(response=>{
    console.log(response);
    this.cancel();
  }, error=>{
    console.log(error);
    this.toatsr.error(error.error);

  })
}
cancel(){
  this.cancelRegister.emit(false);
  }

  loadDoctors(){
    this.doctorService.getDoctors().subscribe(doctors=>{
      this.doctors=doctors;
    })
  }
}
