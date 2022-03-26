import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { officeHours } from 'src/app/_models/officeHours';
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

let tomb:officeHours[]=[];
for (let index = 1; index < 6; index++) {
  let item:officeHours={
day:index,
open:this.officeHoursForm.value[index].open,
close:this.officeHoursForm.value[index].close,
  };
tomb.push(item);
  
}
console.log(tomb);
    this.officeHoursService.officeHoursPut(tomb);
   
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
     

    1:new FormGroup({
       
        open: new FormControl('', [Validators.required]),
        close: new FormControl('', [Validators.required]),
     }),
     2:new FormGroup({
       
      open: new FormControl('', [Validators.required]),
      close: new FormControl('', [Validators.required]),
   }),
   3:new FormGroup({
       
    open: new FormControl('', [Validators.required]),
    close: new FormControl('', [Validators.required]),
 }),
 4:new FormGroup({
       
  open: new FormControl('', [Validators.required]),
  close: new FormControl('', [Validators.required]),
}),
5:new FormGroup({
       
  open: new FormControl('', [Validators.required]),
  close: new FormControl('', [Validators.required]),
}),
     
  
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
