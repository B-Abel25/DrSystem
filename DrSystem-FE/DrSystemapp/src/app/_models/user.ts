import { Doctors } from "./doctor";
export interface User{

    name:string;
    medNumber:string;
    email:string;
    phoneNumber:string;
    DateOfBirth:string;
    city:string;
    PostCode:string;
    Street:string;
    HouseNumber:string;
    Password:string;
    photoUrl:string;   
    doctorId: Doctors[]; 
    
    
 }