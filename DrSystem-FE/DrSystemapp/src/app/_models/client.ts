
import { Doctor } from "./doctor";
export interface Client {
    id: string;
    medNumber: string;
    name: string;
    email: string;
    phoneNumber: string;
    birthDate: string;
    place: any;
    street: string;
    houseNumber: string;
    member: boolean;
    doctor: Doctor;



}