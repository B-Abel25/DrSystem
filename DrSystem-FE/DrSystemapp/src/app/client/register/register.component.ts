import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup,  ValidatorFn,Validators} from '@angular/forms';

import { ToastrService } from 'ngx-toastr';

import { Doctors } from '../../_models/doctor';
import { Places } from '../../_models/places';
import { AccountService } from '../../_services/account.service';

import { DoctorService } from '../../_services/doctor.service';

import { Router } from '@angular/router';



@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  
  @Output() cancelRegister= new EventEmitter();

  
  doctors:Doctors[];
  submitted:boolean=false;
  registerForm:FormGroup;
  validationErrors: string[];
  postCodes:Places[];
  public showPasswordOnPress: boolean;
  
  constructor(private accountService:AccountService, private toatsr:ToastrService, private doctorService:DoctorService, private fb:FormBuilder, private router:Router) { 
    
     
   
  }

  // initializeForm(){
  //   this.registerForm= new FormGroup({
  //     username: new FormControl('', Validators.required),
  //     password: new FormControl('',[Validators.required, Validators.maxLength(12), Validators.minLength(6)]),
  //     confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')]),
  //   })
  //   this.registerForm.controls['password'].valueChanges.subscribe(()=>{
  //     this.registerForm.controls['confirmPassword'].updateValueAndValidity();
  //   })
  // }



  

  ngOnInit() {
   
   

    this.loadDoctors();
    this.loadPostCodes();
    this.initializeForm();
    
  }
  

  onSubmit() {
    this.submitted = true;
    if (this.registerForm.valid) {
      alert('Form Submitted succesfully!!!\n Check the values in browser console.');
      console.table(this.registerForm.value);
    }
  }
    
    
    
   initializeForm(){
    this.registerForm = this.fb.group({
      phoneNumber: ['', [Validators.required,Validators.pattern('[0-9]*'), Validators.maxLength(11), Validators.minLength(11)]],
      medNumber: ['', [Validators.required, Validators.pattern('[0-9]{3}[0-9]{3}[0-9]{3}')], ],
     houseNumber: ['', [Validators.required, Validators.pattern('[0-9 a-z]*')]],
     birthDate: ['', Validators.required],
     street: ['', [Validators.required,Validators.pattern('[a-z A-Z]*')]],
     city: ['', Validators.required],
     placeId: ['', Validators.required],
     doctorId: ['', Validators.required],
     email: ['', [Validators.required, Validators.email,Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")]],
     name: ['', [Validators.required, Validators.pattern('[a-z A-Z]*')]],
     acceptTerms: [false, Validators.requiredTrue],
     password: ['', Validators.compose([Validators.required, Validators.minLength(8), Validators.maxLength(16)])],
     confirmPassword: ['', [Validators.required, this.matchValues('password')]],
     })
     this.registerForm.controls['password'].valueChanges.subscribe(()=>{
       this.registerForm.controls['confirmPassword'].updateValueAndValidity();
     })
   }
  
   
  
  

  matchValues(matchTo:string) : ValidatorFn{
    return (control:AbstractControl | any )=>{
      return control?.value == control?.parent?.controls[matchTo].value ? null : {isMatching: true}
    }
  }
  
  

  
 

register(){
console.log(this.registerForm.value);
  this.accountService.register(this.registerForm.value).subscribe(response=>{
    this.router.navigateByUrl('/drsystem/login');
   
  }, error=>{
    this.validationErrors=error;
    console.log(error)

  })
  
}


  loadDoctors(){
    this.accountService.getDoctors().subscribe(doctors=>{
      this.doctors=doctors;
    
    })
  }
  loadPostCodes(){
    this.accountService.getPlaces().subscribe( postCodes=>{
      this.postCodes= postCodes;
     
    })
  }
  
 
}
