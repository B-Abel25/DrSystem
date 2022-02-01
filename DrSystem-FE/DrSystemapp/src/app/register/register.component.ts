import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn,Validators} from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { ConfirmedValidator } from '../confirmed.validator';
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
  registerForm: FormGroup= new FormGroup({});
  doctors!:Doctors[];
  submitted:boolean=false;
  constructor(private accountService:AccountService, private toatsr:ToastrService, private doctorService:DoctorService, private fb:FormBuilder) { 
    this.registerForm= fb.group ({
      name:['',[Validators.required]],
      password:['',[Validators.required, Validators.minLength(6), Validators.maxLength(12)]],
      confirmPassword:['', [Validators.required]]
     
    },
    {
Validators:this.MustMatch('password', 'confirmPassword'),
    })
  }

  MustMatch(controlName: string, matchingControlName: string){
return (formGroup:FormGroup)=>{
  const control= formGroup.controls[controlName];
  const matchingControl= formGroup.controls[matchingControlName];
  if (matchingControl.errors && !matchingControl.errors['MustMatch']) {
    return
  }
  if (control.value !== matchingControl.value) {
    matchingControl.setErrors({MustMatch:true});
  }
  else {
    matchingControl.setErrors(null);
  }
}
  }

  onSubmit(){
    this.submitted=true;
    if (this.registerForm.invalid) {
      return;
    }
  }
  get validate (){return this.registerForm.controls}

  ngOnInit(): void {
    
    this.loadDoctors();
    this.onSubmit();
  }

  
  
  

  
 

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
