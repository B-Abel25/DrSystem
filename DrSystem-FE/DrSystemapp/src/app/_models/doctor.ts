import { Client } from "./client";

export interface Doctor {
    id: string;
    name: string;
    place: any;
    sealNumber: string;
    phoneNumber: number;
    email: string;
    clients: Client[];
    token: string;

}