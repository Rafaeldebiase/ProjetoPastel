import { IUser } from "src/app/user";
import { Itask } from "./task";

export interface IUserTask {
    userDto: IUser
    task?: Itask[]
}