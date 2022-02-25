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

  getMessages() {
    return this.http.get<Message[]>(this.baseUrl + 'private/doctor/messages');
  }
  getMessageThread() {
    return this.http.get<Message[]>(this.baseUrl + 'private/doctor/messages');
  }

  sendMessage(medNumber: string, content: string) {
    return this.http.post<Message>(
      this.baseUrl + 'private/doctor/message/send',
      { recieverNumber: medNumber, content }
    );
  }

  deleteMessage(id: string) {
    return this.http.delete(this.baseUrl + 'private/user/message/delete/' + id);
  }
}
