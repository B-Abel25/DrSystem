import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Doctor } from 'src/app/_models/doctor';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-complaint',
  templateUrl: './complaint.component.html',
  styleUrls: ['./complaint.component.css']
})
export class ComplaintComponent implements OnInit {
  @ViewChild('messageForm') messageForm: NgForm;
  @Input() doctor: Doctor;
  @Input() messages: Message[];
  messageContent: string;
  constructor(
    private messageService: MessageService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
  
    this.loadMessages();
  }

  loadMessages() {
    this.messageService
      .getMessageThreadClient()
      .subscribe((messages) => {
        this.messages = messages;
      });
  }

  sendMessage() {
    console.log(this.doctor);
    this.messageService
      .sendMessageClient(this.messageContent)
      .subscribe((messages) => {
        this.messages.push(messages);

        this.messageForm.reset();
      });
  }
}
