import { LocationStrategy } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {
  CalendarOptions,
  DateSelectArg,
  EventApi,
  EventClickArg,
} from '@fullcalendar/angular';
import { Calendar, Duration } from '@fullcalendar/core';
import esLocale from '@fullcalendar/core/locales/hu';

import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import timeGridPlugin from '@fullcalendar/timegrid';
import { calendar } from 'ngx-bootstrap/chronos/moment/calendar';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Appointment } from 'src/app/_models/appointment';
import { officeHours } from 'src/app/_models/officeHours';
import { AppointmentService } from 'src/app/_services/appointment.service';
import { OfficeHoursService } from 'src/app/_services/office-hours.service';

declare let $: any;

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css'],
})
export class BookingComponent implements OnInit {
  Events: any[] = [];
  currentDateTimeSent: string;
  appointments: Appointment[];
   eventdate: string;
  successdata: any;
  addEventForm: FormGroup;
  submitted = false;
 
  currentDate = new Date();
  myDate = Date.now();
  duration:number;
  open:string;
  close:string;
  constructor(
    private location: LocationStrategy,
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private appointmentService: AppointmentService,
    private toastr: ToastrService,
    
    private officeHoursService:OfficeHoursService
  ) {
    history.pushState(null, null, window.location.href);
    this.location.onPopState(() => {
      history.pushState(null, null, window.location.href);
    });
  }
  calendarOptions: CalendarOptions = {};

  toggleWeekends() {
    this.calendarOptions.weekends = !this.calendarOptions.weekends; 
  }
  eventClick(event) {
   
  }

  onSubmit() {
    
    this.submitted = true;
    

    this.addEventForm.get('Description').updateValueAndValidity();

    if (this.addEventForm.invalid) {
      return;
    } else {
      $('#myModal').modal('hide');
      
      this.appointmentService.Appointment(this.addEventForm.value).subscribe(
        (response) => {
          this.loadClientAppointments();
          this.toastr.success("Sikeres foglalás!")
        },
        (error) => {
        
          this.toastr.error(error.error);
        }
      );
      this.addEventForm.reset();
    }
  }

  ngOnInit() {
    this.initializationForm();
    this.loadOfficeHours();
   
    this.loadClientAppointments();
   
    
  }
  
  handleDateClick(arg) {
    let time = arg.dateStr.split('T');
    $('#myModal').modal('show');
    $('.modal-title, .eventstarttitle').text('');
    let currentTime = time[1].split('+');
    $('.modal-title').text('Foglalás erre a napra : ' + time[0]);
    $('.eventstarttitle').text(currentTime[0]);
    this.currentDateTimeSent = arg.dateStr;
    
    this.addEventForm = this.formBuilder.group({
      Description: ['', [Validators.required]],

      Start: this.currentDateTimeSent,
    });
  }

  initializationForm() {
      this.addEventForm = this.formBuilder.group({
      Description: ['', [Validators.required]],
      Start: this.currentDateTimeSent,
    });
  }

  loadClientAppointments() {
      this.appointmentService
      .getClientAppointment()
      .subscribe((appointment: any) => {
        this.Events.push(appointment);
        this.calendarOptions.events = appointment;
        this.loadDuration();
        
      });
  }
  loadDuration() {
    this.officeHoursService.getDurationClient().subscribe((durationGet) => {
      this.duration = durationGet;
      this.calendarOptions.slotDuration="00:"+this.duration+":00";
                    
    });
  }
  loadOfficeHours() {
    
    this.officeHoursService.getOfficeHoursClient().subscribe((officeHoursGet) => {
      this.calendarOptions = {
        dateClick: this.handleDateClick.bind(this),
        weekends: false,
        initialView: 'timeGridWeek',
        locale: esLocale,
        timeZone: 'local',
        slotEventOverlap: false,
        //eventMinHeight:2,
        allDaySlot: false,
        height:500,
        eventClick: function (param) {
          //https://github.com/fullcalendar/fullcalendar/issues/5011
         console.log(param)
         
        
         
  
          this.actualAppointment = {
            title: param.event._def.title,
            start: param.event._instance.range.start.toUTCString(),
            end: param.event._instance.range.end.toUTCString(),
            description: param.event._def.extendedProps['description'],
          };
          this.time = this.actualAppointment.start.split(' ');
          if (this.time[0] == 'Wed,') {
            this.time[0] = 'Szerda';
          }
          if (this.time[0] == 'Tue,') {
            this.time[0] = 'Kedd';
          }
          if (this.time[0] == 'Mon,') {
            this.time[0] = 'Hétfő';
          }
          if (this.time[0] == 'Thu,') {
            this.time[0] = 'Csütörtök';
          }
          if (this.time[0] == 'Fri,') {
            this.time[0] = 'Péntek';
          }
          if (this.actualAppointment.title!="Foglalt") {
            $('#myModal2').modal('show');
           }
          $('.modal-title').text(' Foglaló: ' + this.actualAppointment.title);
          $('.eventstarttitle').text(
            this.time[0] +
              ' ' +
              this.time[3] +
              ' ' +
              this.time[2] +
              ' ' +
              this.time[1] +
              ' ' +
              this.time[4]
          );
          $('.eventdescription').text(this.actualAppointment.description);
          this.stringJson = JSON.stringify(this.actualAppointment);
          console.log(this.stringJson);
        },
       
        headerToolbar: {},
       
        validRange: {
          start: this.myDate,
        },
        eventBackgroundColor: '#ffff',
     
        editable: false,
        selectable: false,
        selectMirror: true,
      };
      this.open=officeHoursGet.sort((one, two) => (one.open < two.open ? -1 : 1))[0].open;
      this.close=officeHoursGet.sort((one, two) => (one.close > two.close ? -1 : 1))[0].close;                    
       
      this.calendarOptions.slotMinTime=this.open+":00";
    this.calendarOptions.slotMaxTime=this.close+":00";
  
      });
     
  }
  closeModal()
  {
    $('#myModal2').modal('hide');
  }

  closeAppointmentModal()
  {
    $('#myModal').modal('hide');
  }
}
