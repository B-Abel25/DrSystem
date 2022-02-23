import { HttpClient } from '@angular/common/http';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
baseUrl=environment.apiUrl;
container:any
  constructor(private http:HttpClient) { }

  getMessages(id:string)
  {
 
    return this.http.get<Message[]>(this.baseUrl + 'private/users/messages/' + id );
  }
  getMessageThread(id:string){
return this.http.get<Message[]>(this.baseUrl+'private/messages/thread/'+id);
  }
}
