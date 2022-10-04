import { IPhone } from "./phone";

export interface IResultPhone {
    phones: IPhone[],
    errors: string[]
}