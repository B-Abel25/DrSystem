import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn,Validators} from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { ConfirmedValidator } from '../confirmed.validator';
import { Doctors } from '../_models/doctor';
import { AccountService } from '../_services/account.service';
import { CustomvalidationService } from '../_services/customvalidation.service';
import { DoctorService } from '../_services/doctor.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  
  @Output() cancelRegister= new EventEmitter();
model: any={}
  
  doctors!:Doctors[];
  submitted:boolean=false;
  registerForm!:FormGroup;
  constructor(private accountService:AccountService, private toatsr:ToastrService, private doctorService:DoctorService, private fb:FormBuilder,private customValidator: CustomvalidationService) { 
    
     
   
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

//   MustMatch(controlName: string, matchingControlName: string){
// return (formGroup:FormGroup)=>{
//   const control= formGroup.controls[controlName];
//   const matchingControl= formGroup.controls[matchingControlName];
//   if (matchingControl.errors && !matchingControl.errors['MustMatch']) {
//     return
//   }
//   if (control.value !== matchingControl.value) {
//     matchingControl.setErrors({MustMatch:true});
//   }
//   else {
//     matchingControl.setErrors(null);
//   }
// }
//   }

  // onSubmit(){
  //   this.submitted=true;
  //   if (this.registerForm.invalid) {
  //     return;
  //   }
  // }
  // get validate (){return this.registerForm.controls}

  ngOnInit() {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      username: ['', [Validators.required], this.customValidator.userNameValidator.bind(this.customValidator)],
      password: ['', Validators.compose([Validators.required, this.customValidator.patternValidator()])],
      confirmPassword: ['', [Validators.required]],
    },
      {
        validator: this.customValidator.MustMatch('password', 'confirmPassword'),
      }
    );
  }
  get registerFormControl() {
    return this.registerForm.controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.registerForm.valid) {
      alert('Form Submitted succesfully!!!\n Check the values in browser console.');
      console.table(this.registerForm.value);
    }
  }
    
    
    // this.initializeForm();
   
   
  
  

  // matchValues(matchTo:string) : ValidatorFn{
  //   return (control:AbstractControl | any)=>{
  //     return control?.value == control?.parent?.controls[matchTo].value ? null : {isMatching: true}
  //   }
  // }
  
  

  
 

register(){

  this.accountService.register(this.model).subscribe(response=>{
    console.log(response);
   
  }, error=>{
    console.log(error);
    this.toatsr.error(error.error);

  })
}


  loadDoctors(){
    this.doctorService.getDoctors().subscribe(doctors=>{
      this.doctors=doctors;
    })
  }
}
