import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Component, Input, OnInit } from '@angular/core';
import { Client } from 'src/app/_models/client';
import { User } from 'src/app/_models/user';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-doctor-client-messaging',
  templateUrl: './doctor-client-messaging.component.html',
  styleUrls: ['./doctor-client-messaging.component.css']
})
export class DoctorClientMessagingComponent implements OnInit {
@Input() user: User;
messages:Message[];
  constructor(private messageService:MessageService) { }

  ngOnInit(): void {
  }

  loadMessages()
  {
    this.messageService.getMessageThread(this.user.id).subscribe(messages=>
      {
        this.messages=messages;
      })
  }

}
