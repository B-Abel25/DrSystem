import { HttpClient } from '@angular/common/http';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  baseUrl = environment.apiUrl;
  container: any;
  constructor(private http: HttpClient) {}

  getMessagesDoctor() {
    return this.http.get<Message[]>(this.baseUrl + 'private/doctor/messages');
  }
  getMessageThreadDoctor(medNumber: string) {
    return this.http.get<Message[]>(
      this.baseUrl + 'private/doctor/messages/' + medNumber
    );
  }

  sendMessageDoctor(medNumber: string, content: string) {
    return this.http.post<Message>(
      this.baseUrl + 'private/doctor/message/send',
      { recieverNumber: medNumber, content }
    );
  }


  getMessageThreadClient() {
    return this.http.get<Message[]>(
      this.baseUrl + 'private/client/messages' 
    );
  }

  sendMessageClient( content: string) {
   
    return this.http.post<Message>(
      this.baseUrl + 'private/client/message/send',
      content
    );
  }
  deleteMessage(id: string) {
    return this.http.delete(this.baseUrl + 'private/user/message/delete/' + id);
  }
}
