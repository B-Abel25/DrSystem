import { LocationStrategy } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CalendarOptions,DateSelectArg, EventApi, EventClickArg } from '@fullcalendar/angular';
import { Calendar, Duration } from '@fullcalendar/core';
import esLocale from '@fullcalendar/core/locales/hu';

import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import timeGridPlugin from '@fullcalendar/timegrid';

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
  minTime="10:00:00";
  currentDate = new Date();
  myDate = Date.now();
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
        
        dateClick: this.handleDateClick.bind(this),
        weekends: false,
        initialView: 'timeGridWeek',
        locale: esLocale,
       slotEventOverlap:false,
       eventMinHeight:2,
       allDaySlot: false,
       slotMinTime:this.minTime,
       events: [
        { title: 'event 1', date: '2022-03-01T10:00:00+01:00', color:'red' },
        { title: 'event 2', date: '2020-06-30' }
      ],
        headerToolbar:{
          left: 'prev,next today',
          center: 'title',
          
    
    
        },

        validRange:{
        start:this.myDate
        },
        slotDuration:"00:10:00",
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
    $(".modal-title").text("Foglalás erre a napra : "+arg.dateStr);
    $(".eventstarttitle").text(arg.dateStr);
   
  }
  //Hide Modal PopUp and clear the form validations
  hideForm(){
    this.addEventForm.patchValue({ title : ""});
    this.addEventForm.get('title').clearValidators();
    this.addEventForm.get('title').updateValueAndValidity();
    }
  

}



