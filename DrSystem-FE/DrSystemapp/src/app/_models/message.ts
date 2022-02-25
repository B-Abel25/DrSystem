export interface Message {
  id: string;
  sender: any;
  
  recipentId: any;
  
  content: string;
  dateRead?: Date;
  messageSent: Date;
}
