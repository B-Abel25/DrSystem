export interface Message {
  id: number;
  senderId: number;
  senderUsername: string;
  recipentId: number;
  recipentUsername: string;
  content: string;
  dateRead?: Date;
  messageSent: Date;
}
