import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CalendarOptions } from '@fullcalendar/core';
import esLocale from '@fullcalendar/core/locales/hu';
import { ToastrService } from 'ngx-toastr';
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
  minTime = '10:00:00';
  showModal: boolean;
  name:string;
  date:string;
  appointments:Appointment[];
  Events: any[] = [];
  currentDateTimeSent: string;
  start:Date;
  actualAppointment:Appointment=<Appointment>{};
  
  eventdate: string;
  successdata: any;
  duration: number;
  addEventForm: FormGroup;
  deleteEventForm:FormGroup;
  submitted = false;
  Hours: officeHours;
  time:string;
  description:string;
  currentDate = new Date();
  myDate = Date.now();
  deleteStart:string="";
  constructor(
    private appointmentService: AppointmentService,
    private formBuilder: FormBuilder,
    private officeHoursService: OfficeHoursService,
    private toastr:ToastrService,
  ) {}

  ngOnInit() {
    
    this.initializationForm();
    this.loadDoctorAppointment();
  
   
    
    this.calendarOptions = {
      //dateClick: this.handleDateClick.bind(this),
      weekends: false,
      timeZone: 'Europe/Budapest',
      timeZoneParam:'Europe/Budapest',
      initialView: 'timeGridWeek',
      locale: esLocale,
      slotEventOverlap: false,
      eventMinHeight: 2,
      allDaySlot: false,
      slotMinTime: this.minTime,
     contentHeight:500,
     dateClick: this.handleDateClick.bind(this),
     eventClick:function(param) {
     //https://github.com/fullcalendar/fullcalendar/issues/5011
      $('#myModal2').modal('show');
    
     
      this.actualAppointment={
    
      title:param.event._def.title,
      start:param.event._instance.range.start.toUTCString(),
      end:param.event._instance.range.end.toUTCString(),
      description:param.event._def.extendedProps['description'],
      };
     this.time=this.actualAppointment.start.split(' ');
      if (this.time[0]=='Wed,') {
        this.time[0]='Szerda';
      }
      if (this.time[0]=='Tue,') {
        this.time[0]='Kedd';
      }
      if (this.time[0]=='Mon,') {
        this.time[0]='Hétfő';
      }
      if (this.time[0]=='Thu,') {
        this.time[0]='Csütörtök';
      }
      if (this.time[0]=='Fri,') {
        this.time[0]='Péntek';
      }
  
      $('.modal-title').text(' Foglaló: '+this.actualAppointment.title);
      $('.eventstarttitle').text(this.time[0]+" "+this.time[3]+" "+this.time[2]+" "+this.time[1]+" "+this.time[4]);
      $('.eventdescription').text(this.actualAppointment.description);
   
     

     
    },
     titleFormat: { // will produce something like "Tuesday, September 18, 2018"
      month: 'long',
      year: 'numeric',
      day: 'numeric',
      weekday: 'long'
    },
      
     hiddenDays:[2],
     
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

    
    this.initializationForm();
    
  }

 

  eventClick(param:any) {
   
  
  
  }

  hide() {
    this.showModal = false;
  }

  ShowModal() {
    this.showModal = !this.showModal;
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
    
    this.addEventForm = this.formBuilder.group({
      Description: new FormControl(''),

      Start:new FormControl(this.currentDateTimeSent),
    });
  }
  initializationDeleteForm() {
   
    this.deleteEventForm = this.formBuilder.group({
     

      Start:new FormControl('',[]),
     
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
    
       this.Hours=officeHoursGet;
                          
    });
  }

  deleteAppointment() {
   

    
    this.appointmentService.deleteAppointmentDoctor(this.addEventForm.value);
    
    this.toastr.success('Sikeresen elutasította a kérelmet!');
  }
  
}
