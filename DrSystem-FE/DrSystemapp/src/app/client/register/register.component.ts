import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder,  FormGroup,  ValidatorFn,Validators} from '@angular/forms';

import { ToastrService } from 'ngx-toastr';

import { Doctor } from '../../_models/doctor';
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

  doctors:Doctor[];
  submitted:boolean=false;
  registerForm:FormGroup;
  validationErrors: string[];
  places:Places[];
  public showPasswordOnPress: boolean;
  showMsg: boolean = false;
  
  
  constructor(private accountService:AccountService, private toatsr:ToastrService, private doctorService:DoctorService, private fb:FormBuilder, private router:Router) { 
  
  }

 



  

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
      phoneNumber: ['', [Validators.required,Validators.pattern('[0-9]*'), Validators.maxLength(11), Validators.minLength(9)]],

      medNumber: ['', [Validators.required, Validators.pattern('[0-9]{3}[0-9]{3}[0-9]{3}')], ],


     placeId: ['', Validators.required],
     doctor: ['', Validators.required],
     
     houseNumber: ['', [Validators.required, Validators.pattern('[0-9 a-z]*')]],
     birthDate: ['', Validators.required],
     street: ['', [Validators.required,Validators.pattern('[a-z A-Z áéűúőóüöíÁÉŰÚŐÓÜÖÍ]*')]],
     city: ['', Validators.required],
     postCode: ['', Validators.required],
     doctorId: ['', [Validators.required]],
     email: ['', [Validators.required, Validators.email,Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")]],
     name: ['', [Validators.required, Validators.pattern('[a-z A-Z áéűúőóüöíÁÉŰÚŐÓÜÖÍ]*')]],
     acceptTerms: [false, Validators.requiredTrue],
     password: ['', Validators.compose([Validators.required, Validators.minLength(8), Validators.maxLength(16)])],
     confirmPassword: ['', [Validators.required, this.matchValues('password')]],
     })
/*Itt nézz körül*/
     this.registerForm.controls['postCode'].valueChanges.subscribe(x=>{
      x = x+"";
       if (x.length == 4)
       {
         console.log(x);
        this.registerForm.controls['city'].setValue(this.places.find(y => y.postCode == x).city.name);
       }
       else
       {
        this.registerForm.controls['city'].setValue("");
       }
      })

      this.registerForm.controls['city'].valueChanges.subscribe(x=>{
        
         let  exist = this.places.find(y => y.city.name == x && y.postCode == this.registerForm.controls['postCode'].value)
         console.log(exist); 
         if(exist != null)
          {
            this.registerForm.controls['placeId'].setValue(exist.id);
          }
          else{
            this.registerForm.controls['placeId'].setValue("");
          }
         
         
        })


this.registerForm.controls['doctor'].valueChanges.subscribe(x=>{

  let  exist = this.doctors.find(y => y.name +" " + "-" +" "+ y.place.postCode == x);
         console.log(exist); 
         if(exist!= null) this.registerForm.controls['doctorId'].setValue(exist.id);
         else  this.registerForm.controls['doctorId'].setValue("");
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

    this.router.navigateByUrl('/login');
    this.showMsg= true;
   

   
  }, error=>{
    this.validationErrors=error;
    console.log(error)

  })
  
}


  loadDoctors(){
    this.accountService.getDoctors().subscribe(doctors=>{
      this.doctors=doctors.sort((one, two) => (one.name < two.name ? -1 : 1));
    
    })
  }
  loadPostCodes(){
    this.accountService.getPlaces().subscribe( postCodes=>{
      this.places= postCodes;
     
    })
  }
}
