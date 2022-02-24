
import { Doctor } from "./doctor";
import { Place } from "./place";
export interface Client {
    id: string;
    medNumber: string;
    name: string;
    email: string;
    phoneNumber: string;
    birthDate: string;
    place: Place;
    street: string;
    houseNumber: string;
    member: boolean;
    doctor: Doctor;



}