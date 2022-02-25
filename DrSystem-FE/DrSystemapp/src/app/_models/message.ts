export interface Message {
  id: string;
  sender: any;
  reciever: any;
  content: string;
  dateRead?: Date;
  messageSent: string;
}
