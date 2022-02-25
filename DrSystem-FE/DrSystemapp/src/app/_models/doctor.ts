import { Client } from "./client";
import { User } from "./user";

export interface Doctor extends User {
  
    sealNumber: string;
    
   
    clients: Client[];
   

}