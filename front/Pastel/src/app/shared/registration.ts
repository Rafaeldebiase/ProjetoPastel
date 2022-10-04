import { IPhone } from "./phone";


export interface IRegistration {
    id: string,
    firstName: string,
    lastName: string,
    email: string,
    password: string,
    phones: IPhone[],
    street: string,
    streetNumber: number,
    streetComplement: string,
    neighborhood: string,
    city: string,
    state: string,
    country: string,
    zipCode: string,
    birthDate: Date,
    role: string,
    managerId: string
}
