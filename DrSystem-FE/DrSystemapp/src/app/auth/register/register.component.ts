import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  successMessage:string =""

  regForm!:FormGroup

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.regForm = this.fb.group({
      name: ['',[Validators.required]],
    
      TAJ: ['',[Validators.required,Validators.pattern("[0-9]")]],
      mobileNumber: ['',[Validators.required, Validators.min(1000000000),Validators.max(9999999999)]],
      email:['',[Validators.required, Validators.pattern("[a-zA-Z0-9]*@gmail.com")]],
      password: ['',[Validators.required,Validators.pattern("[a-zA-z@_]{6,}")]]
      
    })
  }

  register(){
    this.successMessage = "Successfully Registered..."
    // console.log(this.regForm)
  }

}
