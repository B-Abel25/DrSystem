import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CalendarOptions } from '@fullcalendar/core';
import esLocale from '@fullcalendar/core/locales/hu';
import { Appointment } from 'src/app/_models/appointment';
import { officeHours } from 'src/app/_models/officeHours';
import { AppointmentService } from 'src/app/_services/appointment.service';
import { OfficeHoursService } from 'src/app/_services/office-hours.service';
declare let $: any;
@Component({
  selector: 'app-appointment-list',
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.css'],
})

export class AppointmentListComponent implements OnInit {
  
  calendarOptions: CalendarOptions = {};
  minTime="10:00:00";
  showModal: boolean;
  name:string;
  date:string;
  appointments:Appointment[];
  Events: any[] = [];
  currentDateTimeSent: string;
  
  acctualEvent:any;
  get f() {
    return this.addEventForm.controls;
  }
  eventdate: string;
  successdata: any;
  duration:number;
  addEventForm: FormGroup;
  submitted = false;
  Hours:officeHours;
  
  currentDate = new Date();
  myDate = Date.now();
  constructor(private appointmentService:AppointmentService, private formBuilder:FormBuilder, private officeHoursService:OfficeHoursService) { }

  ngOnInit() {
    this.initializationForm();
    
    
    this.loadDoctorAppointment();
   
   
    console.log(this.duration)
    this.calendarOptions = {
      //dateClick: this.handleDateClick.bind(this),
      weekends: false,
      initialView: 'timeGridWeek',
      locale: esLocale,
      slotEventOverlap: false,
      eventMinHeight: 2,
      allDaySlot: false,
      slotMinTime: this.minTime,
     contentHeight:500,
     dateClick: this.handleDateClick.bind(this),
     eventClick(param:any) {
      $('#myModal2').modal('show');
      
   this.acctualEvent=param;
   console.log(this.acctualEvent);
   console.log(this.acctualEvent.event._def.extendedProps.description);
    
    },
     titleFormat: { // will produce something like "Tuesday, September 18, 2018"
      month: 'long',
      year: 'numeric',
      day: 'numeric',
      weekday: 'long'
    },
      
     hiddenDays:[2],
      events: [
        { title: '', start: '2022-03-21T10:00:00+01:00', end:'2022-03-21T10:10:00+01:00', color:'red' },
        { title: 'event 2', date: '2022-03-21T11:00:00+01:00-11:10:00+01:00', color:'yellow' }
      ],
      themeSystem: 'bootstrap5',
    
      editable: false,
      selectable: true,
      selectMirror: true,
      droppable: false,
      selectOverlap: false,
    };

    
  }
  handleDateClick(arg) {
    let time = arg.dateStr.split('T');

    $('#myModal').modal('show');
    $('.modal-title, .eventstarttitle').text('');
    let currentTime = time[1].split('+');
    $('.modal-title').text('Foglalás erre a napra : ' + time[0]);

    $('.eventstarttitle').text(currentTime[0]);
    this.currentDateTimeSent = arg.dateStr;

    console.log(this.currentDateTimeSent);
    this.initializationForm();
  }


  eventClick(model:any) {
    
   this.setValues();
  
  
  }
  
  hide()
{
  this.showModal = false;
}

ShowModal()
{
this.showModal=!this.showModal;
}


  loadDoctorAppointment() {
    this.appointmentService
      .getDoctorAppointment()
      .subscribe((appointment) => {
        this.appointments = appointment;
      
       this.calendarOptions.events = appointment;
       
      });
      this.loadOfficeHours();
      this.loadDuration();
    
      
  }


  onSubmit() {
    
    this.submitted = true;
    // stop here if form is invalid and reset the validations

    this.addEventForm.get('Description').updateValueAndValidity();

    if (this.addEventForm.invalid) {
      return;
    } else {
      $('#myModal').modal('hide');
      
      this.appointmentService.AppointmentDoctor(this.addEventForm.value).subscribe(
        (response) => {
          this.loadDoctorAppointment();
        },
        (error) => {
          console.log(error);
          
        }
      );
      this.addEventForm.reset();
    }
  }
  hideForm() {
    this.addEventForm.patchValue({ title: '' });
    this.addEventForm.get('Description').clearValidators();
    this.addEventForm.get('Description').updateValueAndValidity();
  }


  initializationForm() {
    //console.log(this.currentDateTimeSent);
    this.addEventForm = this.formBuilder.group({
      Description: new FormControl(''),

      Start:new FormControl(this.currentDateTimeSent),
    });
  }


  loadDuration() {
    this.officeHoursService.getDuration().subscribe((durationGet) => {
      this.duration = durationGet;
      this.calendarOptions.slotDuration="00:"+this.duration+":00";
                           
    });
  }

  loadOfficeHours() {
    this.officeHoursService.getOfficeHours().subscribe((officeHoursGet) => {
    
        this.Hours = officeHoursGet;
                          
    });
  }


  setValues()
  {


 
    this.addEventForm.controls['Description'].setValue(
      this.acctualEvent.event._def.extendedProps.description
    );
    console.log(this.acctualEvent.events._def.extendedProps.description);
    this.addEventForm.controls['Start'].setValue(this.appointments[0].start);
    console.log(this.addEventForm.controls);
    
  
    
  }
}
