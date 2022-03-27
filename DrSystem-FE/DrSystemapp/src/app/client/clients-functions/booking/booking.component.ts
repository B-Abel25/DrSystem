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
  modalRef!: BsModalRef;
  get f() {
    return this.addEventForm.controls;
  }
  eventdate: string;
  successdata: any;
  addEventForm: FormGroup;
  submitted = false;
  minTime = '10:00:00';
  slotDuration = '00:10:00';
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
    private modalService: BsModalService,
    private officeHoursService:OfficeHoursService
  ) {
    history.pushState(null, null, window.location.href);
    this.location.onPopState(() => {
      history.pushState(null, null, window.location.href);
    });
  }
  calendarOptions: CalendarOptions = {};

  toggleWeekends() {
    this.calendarOptions.weekends = !this.calendarOptions.weekends; // toggle the boolean!
  }
  eventClick(event) {
    //console.log(event);
  }

  onSubmit() {
    console.log(this.addEventForm.value);
    this.submitted = true;
    // stop here if form is invalid and reset the validations

    this.addEventForm.get('Description').updateValueAndValidity();

    if (this.addEventForm.invalid) {
      return;
    } else {
      $('#myModal').modal('hide');
      console.log(this.addEventForm.value);
      this.appointmentService.Appointment(this.addEventForm.value).subscribe(
        (response) => {
          this.loadClientAppointments();
        },
        (error) => {
          console.log(error);
          this.toastr.error(error.error);
        }
      );
      this.addEventForm.reset();
    }
  }

  ngOnInit() {
    this.initializationForm();
    this.loadOfficeHours();
    console.log(this.open);
    console.log(this.close);
    console.log('ott');
    this.loadClientAppointments();
    console.log('itt');
    
  }
  //Show Modal with Forn on dayClick Event
  handleDateClick(arg) {
    let time = arg.dateStr.split('T');

    $('#myModal').modal('show');
    $('.modal-title, .eventstarttitle').text('');
    let currentTime = time[1].split('+');
    $('.modal-title').text('Foglalás erre a napra : ' + time[0]);

    $('.eventstarttitle').text(currentTime[0]);
    this.currentDateTimeSent = arg.dateStr;

    console.log(this.currentDateTimeSent);
    this.addEventForm = this.formBuilder.group({
      Description: ['', [Validators.required]],

      Start: this.currentDateTimeSent,
    });
  }

  //Hide Modal PopUp and clear the form validations
  hideForm() {
    this.addEventForm.patchValue({ title: '' });
    this.addEventForm.get('Description').clearValidators();
    this.addEventForm.get('Description').updateValueAndValidity();
  }
  initializationForm() {
    //console.log(this.currentDateTimeSent);
    this.addEventForm = this.formBuilder.group({
      Description: ['', [Validators.required]],

      Start: this.currentDateTimeSent,
    });
  }
  loadClientAppointments() {
    console.log('kiderül az igazság');
    this.appointmentService
      .getClientAppointment()
      .subscribe((appointment: any) => {
        this.Events.push(appointment);
        console.log(appointment);
        console.log(this.Events);
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
    //this.open=Math.min(officeHoursGet.map(x=>x.open));
       this.open=officeHoursGet.sort((one, two) => (one.open < two.open ? -1 : 1))[0].open;
       this.close=officeHoursGet.sort((one, two) => (one.close > two.close ? -1 : 1))[0].close;                    
    console.log(this.open);
    console.log(this.close);
    this.calendarOptions = {
      dateClick: this.handleDateClick.bind(this),
      weekends: false,
      initialView: 'timeGridWeek',
      locale: esLocale,
      timeZone: 'local',
      slotEventOverlap: false,
      //eventMinHeight:2,
      allDaySlot: false,
      slotMinTime: this.open+":00",
      slotMaxTime:this.close+":00",
      eventClick: function (arg) {
        alert(arg.event.title);
        alert(arg.event.start);
      },
     
      headerToolbar: {},

      validRange: {
        start: this.myDate,
      },
      eventBackgroundColor: '#ffff',
      slotDuration: this.slotDuration,
      editable: false,
      selectable: false,
      selectMirror: true,
    };
      });
  }
}
