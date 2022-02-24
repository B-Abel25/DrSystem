import { Doctor } from "./doctor";
import { Place } from "./place"
export interface Registration {

    medNumber: string;
    token: string;
    name: string;
    email: string;
    phoneNumber: string;
    birthDate: string;
    placeId: Place[];
    street: string;
    houseNumber: string;
    doctorId: Doctor[];


}