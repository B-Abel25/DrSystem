import { Doctors } from "./doctor";
export interface User{

    name:string;
    medNumber:string;
    email:string;
    phoneNumber:string;
    birthDate:string;
    city:string;
    placeId:string;
    street:string;
    houseNumber:string;
    password:string;
    photoUrl:string;   
    doctorId: Doctors[]; 
    
    
 }