import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { OfficeHoursService } from 'src/app/_services/office-hours.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  officeHoursForm: FormGroup;
  durationForm: FormGroup;
  constructor(private officeHoursService:OfficeHoursService, private toastr:ToastrService, private fb: FormBuilder,) { }

  ngOnInit() {
    this.officeHoursinitializationForm();
  }
  officeHours() {
    this.officeHoursService.officeHours(this.officeHoursForm.value).subscribe(
      (response) => {
        
      },
      (error) => {
        console.log(error);
        this.toastr.error(error.error);
      }
    );
  }

  duration() {
    this.officeHoursService.Duration(this.durationForm.value).subscribe(
      (response) => {
        
      },
      (error) => {
        console.log(error);
        this.toastr.error(error.error);
      }
    );
  }
  officeHoursinitializationForm() {
    this.officeHoursForm = this.fb.group({
      medNumber: ['', [Validators.required, Validators.pattern('[0-9]{3}[0-9]{3}[0-9]{3}')],],
      password: ['', Validators.compose([Validators.required, Validators.minLength(8), Validators.maxLength(16)])],
    })
  }
  durationInitializationForm() {
    this.durationForm = this.fb.group({
      duration: ['', [Validators.required, Validators.pattern('[0-9]')],],
      
    })
  }
}
