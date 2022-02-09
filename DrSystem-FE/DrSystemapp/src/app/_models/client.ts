import {Places} from "./places"

export interface Client{
    id:string;
    medNumber:string;
    name:string;
    email:string;
    phoneNumber:string;
    birthDate:string;
    placeId:Places[];
    street:string;
    houseNumber:string;
    
    
    
 }