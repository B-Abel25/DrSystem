
import { Doctor } from "./doctor";
import { Place } from "./place";
import { User } from "./user";
export interface Client extends User{
    
    medNumber: string;
   
   
    member: boolean;
    doctor: Doctor;



}