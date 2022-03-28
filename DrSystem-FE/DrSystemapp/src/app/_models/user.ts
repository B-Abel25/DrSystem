import { Place } from "./place"
export interface User{
    id:string;
    name:string;
    email:string;
    phoneNumber:string;
    place:Place;
    street: string;
    houseNumber: string;
    birthDate:string;
    token:string;
}