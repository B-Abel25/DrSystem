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
  Hours:officeHours;
  duration:number;
  constructor(private officeHoursService:OfficeHoursService, private toastr:ToastrService, private fb: FormBuilder,) { }

  ngOnInit() {
    this.officeHoursinitializationForm();
    this.durationInitializationForm();
    this.loadOfficeHours();
    this.loadDuration();
    
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

 
  officeHoursinitializationForm() {
    this.officeHoursForm = this.fb.group({
     

    1:new FormGroup({
       
        open: new FormControl('',),
        close: new FormControl('', ),
     }),
     2:new FormGroup({
       
      open: new FormControl('', ),
      close: new FormControl('', ),
   }),
   3:new FormGroup({
       
    open: new FormControl('', ),
    close: new FormControl('', ),
 }),
 4:new FormGroup({
       
  open: new FormControl('', ),
  close: new FormControl('', ),
}),
5:new FormGroup({
       
  open: new FormControl('', ),
  close: new FormControl('', ),
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
durationSubmit(){
  console.log(this.durationForm.value);
this.officeHoursService.durationPut(this.durationForm.value['duration']);

}
 
  durationInitializationForm() {
    this.durationForm = this.fb.group({
      duration: ['', [Validators.required, Validators.pattern('[0-9]*')],],
      
    })
  }

  loadOfficeHours() {
    this.officeHoursService.getOfficeHours().subscribe((officeHoursGet) => {
      this.Hours = officeHoursGet;
      console.log(officeHoursGet)
      console.log(this.Hours);
      console.log('komment2');
      this.setHoursControlValues();
    });
  }

  loadDuration() {
    this.officeHoursService.getDuration().subscribe((durationGet) => {
      this.duration = durationGet;
      console.log(durationGet)
      
      console.log('komment2');
      this.setDurationControlValues();
    });
  }
setDurationControlValues()
{
  this.durationForm.controls['duration'].setValue(this.duration)
}
  setHoursControlValues() {
   console.log(this.officeHoursForm.controls['1']);
  
     
   this.officeHoursForm.setValue({
    1:{
      open:this.Hours[0].open,
      close:this.Hours[0].close,
    } , 
    2:{
      open:this.Hours[1].open,
      close:this.Hours[1].close,
    } ,
    3:{
      open:this.Hours[2].open,
      close:this.Hours[2].close,
    } ,
    4:{
      open:this.Hours[3].open,
      close:this.Hours[3].close,
    } ,
    5:{
      open:this.Hours[4].open,
      close:this.Hours[4].close,
    } ,
  });
  }
}
