import { Doctor } from "./doctor";
import { Place } from "./place"
import { User } from "./user";
export interface Registration extends User {

    medNumber: string;
    birthPlace:string;
    motherName:string;
    doctor: Doctor[];


}