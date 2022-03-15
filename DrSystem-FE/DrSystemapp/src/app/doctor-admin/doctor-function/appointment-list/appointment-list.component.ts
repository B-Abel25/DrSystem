import { Component, OnInit } from '@angular/core';
import { CalendarOptions } from '@fullcalendar/core';
import esLocale from '@fullcalendar/core/locales/hu';
@Component({
  selector: 'app-appointment-list',
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.css']
})
export class AppointmentListComponent implements OnInit {
  calendarOptions: CalendarOptions = {};
  minTime="10:00:00";
  constructor() { }

  ngOnInit() {
    this.calendarOptions = {
        
      //dateClick: this.handleDateClick.bind(this),
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

      themeSystem: 'bootstrap5',
      slotDuration:"00:10:00",
      editable: true,
      selectable: true,
      selectMirror: true,
    
  };
  }

}
