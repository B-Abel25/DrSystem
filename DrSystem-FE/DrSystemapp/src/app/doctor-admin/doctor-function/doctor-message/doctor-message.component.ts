import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-doctor-message',
  templateUrl: './doctor-message.component.html',
  styleUrls: ['./doctor-message.component.css']
})
export class DoctorMessageComponent implements OnInit {
messages:Message[];
container:any;
  constructor(private messageService:MessageService, private route:ActivatedRoute) { }

  ngOnInit(): void {
  }
loadMessages(){
  this.messageService.getMessages(this.route.snapshot.paramMap.get('id')).subscribe(message=>{
    this.messages=message;
  })
}
}
