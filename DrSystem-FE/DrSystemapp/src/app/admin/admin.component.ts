import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  successMessage:string ="";
  adminForm!: FormGroup; 
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.adminForm = this.fb.group({
      Admin:['',[Validators.required, Validators.pattern("[A-Za-z0-9]*@gmail.com")]],
      password:['',[Validators.required,Validators.pattern("[A-Za-z0-9@!_]{6,}")]]
    })
  }

  login(){
    this.successMessage="Successfully Loggined In..."
  }

}

