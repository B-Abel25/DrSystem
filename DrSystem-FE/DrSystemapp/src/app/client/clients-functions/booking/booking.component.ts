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
import { calendar } from 'ngx-bootstrap/chronos/moment/calendar';
import { ToastrService } from 'ngx-toastr';
import { AppointmentService } from 'src/app/_services/appointment.service';

declare let $: any;


@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css'],
  
})
export class BookingComponent implements OnInit {
   Events: any[] = [];
  currentDateTimeSent:string;
  get f() { return this.addEventForm.controls; }
  eventdate:string;
  successdata:any;
  addEventForm: FormGroup;
  submitted = false;
  minTime="10:00:00";
  currentDate = new Date();
  myDate = Date.now();
  constructor(private location: LocationStrategy,private formBuilder: FormBuilder, private http: HttpClient, private appointmentService:AppointmentService, private toastr:ToastrService) {
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
  console.log("Hello")
  console.log(this.addEventForm.value)
    this.submitted = true;
    // stop here if form is invalid and reset the validations
   
    this.addEventForm.get('Description').updateValueAndValidity();
  
    if (this.addEventForm.invalid) {
        return;      
      }

      else{
      
        this.appointmentService.Appointment(this.addEventForm.value).subscribe(response => {
          
    
          
        }, error => {
          console.log(error);
          this.toastr.error(error.error);
          
        })
      }
  }
      
    
    ngOnInit() {
      this.calendarOptions = {
       
        dateClick: this.handleDateClick.bind(this),
        weekends: false,
        initialView: 'timeGridWeek',
        locale: esLocale,
        timeZone:'local',
       slotEventOverlap:false,
       eventMinHeight:2,
       allDaySlot: false,
       slotMinTime:this.minTime,
       eventClick:function(arg){
        alert(arg.event.title)
        alert(arg.event.start)
      },
       events: [
        { title: 'Igy kaphatom meg az event adatait', date: '2022-03-02T10:00:00+01:00', color:'red' },
        { title: 'event 2', date: this.myDate }
      ],
        headerToolbar:{
          left: 'prev,next today',
          center: 'title',
          
    
    
        },
       
        validRange:{
        start:this.myDate
        },
        eventBackgroundColor:"#ffff",
        slotDuration:"00:10:00",
        editable: true,
        selectable: true,
        selectMirror: true,
      
    };
    
    this.initializationForm();
    

      
          
  }
  //Show Modal with Forn on dayClick Event
  handleDateClick(arg) {
    let time=arg.dateStr.split('T');
   
    $("#myModal").modal("show");
    $(".modal-title, .eventstarttitle").text("");
    let currentTime=time[1].split('+');
    $(".modal-title").text("Foglalás erre a napra : "+time[0]);
    $(".eventstarttitle").text(currentTime[0]);
    this.currentDateTimeSent=arg.dateStr;
   
    console.log(this.currentDateTimeSent);
    this.addEventForm = this.formBuilder.group({
      Description: ['', [Validators.required]],
    
    Date:this.currentDateTimeSent
      });
  }
  
  
  //Hide Modal PopUp and clear the form validations
   hideForm(){
    this.addEventForm.patchValue({ title : ""});
   this.addEventForm.get('Description').clearValidators();
    this.addEventForm.get('Description').updateValueAndValidity();
    }
    initializationForm() {
      console.log(this.currentDateTimeSent);
      this.addEventForm = this.formBuilder.group({
        Description: ['', [Validators.required]],
      
      Date:this.currentDateTimeSent
        });
    }

}



