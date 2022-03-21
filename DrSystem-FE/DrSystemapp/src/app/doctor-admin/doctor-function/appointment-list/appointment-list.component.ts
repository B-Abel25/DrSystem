import { Component, OnInit } from '@angular/core';
import { CalendarOptions } from '@fullcalendar/core';
import esLocale from '@fullcalendar/core/locales/hu';
import { Appointment } from 'src/app/_models/appointment';
import { AppointmentService } from 'src/app/_services/appointment.service';
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
  appointment:Appointment[];
  constructor(private appointmentService:AppointmentService) { }

  ngOnInit() {
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
      headerToolbar: {
      
      },
      // eventClick:function(arg){
        
   
      //   $("#myModal").modal("show");
      //   $(".modal-title, .eventstarttitle").text("");
       
      //   $(".modal-title").text("Foglalás erre a napra :");
       
        
      //   $(".eventstarttitle").text("Hello");
        
      // },
      eventClick:function(arg){
        alert(arg.event.title)
        alert(arg.event.start)
      },
      events: [
        { title: '', start: '2022-03-21T10:00:00+01:00', end:'2022-03-21T10:10:00+01:00', color:'red' },
        { title: 'event 2', date: '2022-03-21T11:00:00+01:00-11:10:00+01:00', color:'yellow' }
      ],
      themeSystem: 'bootstrap5',
      slotDuration: '00:10:00',
      editable: false,
      selectable: true,
      selectMirror: true,
      droppable: false,
      selectOverlap: false,
    };

    this.loadDoctorAppontment();
  }

  loadDoctorAppontment() {
    this.appointmentService
      .getDoctorAppointment()
      .subscribe((appointment) => {
        this.appointment = appointment;
       console.log(this.appointment)
      });
  }
}
