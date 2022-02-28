import { LocationStrategy } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CalendarOptions } from '@fullcalendar/angular';
import { Calendar } from '@fullcalendar/core';
import esLocale from '@fullcalendar/core/locales/hu';
import timeGridPlugin from '@fullcalendar/timegrid';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import bootstrap5Plugin from '@fullcalendar/bootstrap5';
declare let $: any;


@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css'],
  
})
export class BookingComponent implements OnInit {
   Events: any[] = [];
  
  get f() { return this.addEventForm.controls; }
  eventdate:string;
  successdata:any;
  addEventForm: FormGroup;
  submitted = false;
  constructor(private location: LocationStrategy,private formBuilder: FormBuilder, private http: HttpClient) {
    history.pushState(null, null, window.location.href);
    this.location.onPopState(() => {
      history.pushState(null, null, window.location.href);
    });

    
  }
  calendarOptions: CalendarOptions = {};
 
  toggleWeekends() {
    this.calendarOptions.weekends = !this.calendarOptions.weekends // toggle the boolean!
  }
  eventClick(event){
    console.log(event);
  }

  onSubmit() {
  
    this.submitted = true;
    // stop here if form is invalid and reset the validations
    this.addEventForm.get('title').setValidators([Validators.required]);
    this.addEventForm.get('title').updateValueAndValidity();
    
   //Form Submittion and send data via API
   if(this.submitted)
    {
      // Initialize Params Object
      var myFormData = new FormData();
    
      // Begin assigning parameters
     
         myFormData.append('title', this.addEventForm.value.title);
         myFormData.append('startdate', this.eventdate);
     
       //post request
       
       
           
        
         
   
    }
  }
      
    
    ngOnInit() {
      this.calendarOptions = {
        initialView: 'dayGridMonth',
        dateClick: this.handleDateClick.bind(this),
        weekends: false,
        
        locale: esLocale,
       
        headerToolbar:{
          left: 'prev,next today ',
          center: 'title',
          right: 'dayGridMonth,dayGridWeek,dayGridDay'
    
    
        },
       
       
        editable: true,
        selectable: true,
        selectMirror: true,
      
    };
    //Add User form validations
    this.addEventForm = this.formBuilder.group({
      title: ['', [Validators.required]],
      Date:['',Validators.required]
      });
  }
  //Show Modal with Forn on dayClick Event
  handleDateClick(arg) {
    $("#myModal").modal("show");
    $(".modal-title, .eventstarttitle").text("");
    $(".modal-title").text("Add Event at : "+arg.dateStr);
    $(".eventstarttitle").text(arg.dateStr);
   
  }
  //Hide Modal PopUp and clear the form validations
  hideForm(){
    this.addEventForm.patchValue({ title : ""});
    this.addEventForm.get('title').clearValidators();
    this.addEventForm.get('title').updateValueAndValidity();
    }
  

}



