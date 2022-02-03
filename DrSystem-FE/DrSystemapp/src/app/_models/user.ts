import { Doctors } from "./doctor";
import {Places} from "./places"
export interface User{

    name:string;
    medNumber:string;
    email:string;
    phoneNumber:string;
    birthDate:string;
    city:Places[];
    placeId:Places[];
    street:string;
    houseNumber:string;
    password:string;
    photoUrl:string;   
    doctorId: Doctors[]; 
    
    
 }