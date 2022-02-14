import { Doctors } from "./doctor";
import {Places} from "./places"
export interface User{

    medNumber:string;
    token:string;
    name:string;
    email:string;
    phoneNumber:string;
    birthDate:string;
    placeId:Places[];
    street:string;
    houseNumber:string;
    doctorId: Doctors[]; 
    
    
 }