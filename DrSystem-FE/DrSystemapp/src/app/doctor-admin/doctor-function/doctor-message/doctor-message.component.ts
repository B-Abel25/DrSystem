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
container='Unread';
  constructor(private messageService:MessageService, private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.loadMessages();
  }
loadMessages(){
  this.messageService.getMessages().subscribe(response=>{
    this.messages=response;
  })
}
deleteMessage(id:string)
{
  this.messageService.deleteMessage(id).subscribe(()=>{
    this.messages.splice(this.messages.findIndex(x=>x.id===id),1);
  })
}
}
