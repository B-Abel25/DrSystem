import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Client } from 'src/app/_models/client';
import { User } from 'src/app/_models/user';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-doctor-client-messaging',
  templateUrl: './doctor-client-messaging.component.html',
  styleUrls: ['./doctor-client-messaging.component.css'],
})
export class DoctorClientMessagingComponent implements OnInit {
  @ViewChild('messageForm') messageForm: NgForm;
  @Input() client: Client;
  @Input() messages: Message[];
  messageContent: string;
  constructor(
    private messageService: MessageService,
    private route: ActivatedRoute
  ) {
    
  }

  ngOnInit() {
    this.client = JSON.parse(localStorage.getItem('clients')).find(
      (item) => item.medNumber === this.route.snapshot.paramMap.get('medNumber')
    );
    this.loadMessages();
    console.log(this.client.id)
  }

  loadMessages() {
    this.messageService
      .getMessageThreadDoctor(this.client.medNumber)
      .subscribe((messages) => {
        this.messages = messages;
      });
  }

  sendMessage() {
    console.log(this.client);
    this.messageService
      .sendMessageDoctor(this.client.medNumber, this.messageContent)
      .subscribe((messages) => {
        this.messages.push(messages);

        this.messageForm.reset();
      });
      
  }
}
