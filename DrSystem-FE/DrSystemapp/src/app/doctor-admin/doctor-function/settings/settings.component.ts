import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
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
    console.log(this.officeHoursForm.value);
  }
  officeHours() {
console.log(this.officeHoursForm.value)
    this.officeHoursService.officeHours(this.officeHoursForm.value).subscribe(
      (response) => {
        
      },
      (error) => {
        console.log(error);
        this.toastr.error(error.error);
      }
    );
    this.officeHoursService.Duration(this.durationForm.value).subscribe(
      (response) => {
        
      },
      (error) => {
        console.log(error);
        this.toastr.error(error.error);
      }
    );
  }

  // duration() {
  //   this.officeHoursService.Duration(this.durationForm.value).subscribe(
  //     (response) => {
        
  //     },
  //     (error) => {
  //       console.log(error);
  //       this.toastr.error(error.error);
  //     }
  //   );
  // }
  officeHoursinitializationForm() {
    this.officeHoursForm = this.fb.group({
      tomb:this.fb.array([ 

      {
        day: '1',
        open: new FormControl('', [Validators.required]),
        close: new FormControl('', [Validators.required]),
      },
      {
        day: new FormControl('2'),
        open: new FormControl('', [Validators.required]),
        close: new FormControl('', [Validators.required]),
      },
     {
        day: new FormControl('3'),
        open: new FormControl('', [Validators.required]),
        close: new FormControl('', [Validators.required]),
      },
     {
        day: new FormControl('4'),
        open: new FormControl('', [Validators.required]),
        close: new FormControl('', [Validators.required]),
      },
     {
        day: new FormControl('5'),
        open: new FormControl('', [Validators.required]),
        close: new FormControl('', [Validators.required]),
      }
     
    ])
  })
  }
  get hours() : FormArray {
    return this.officeHoursForm.get("tomb") as FormArray
  }
  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl | any) => {
      return control?.value !== control?.parent?.controls[matchTo].value
        ? null
        : { isMatching: true };
    };
  }

 
  durationInitializationForm() {
    this.durationForm = this.fb.group({
      duration: ['', [Validators.required, Validators.pattern('[0-9]')],],
      
    })
  }
}
