import { IUser } from "src/app/shared/user";
import { Itask } from "./task";

export interface IUserTask {
    userDto: IUser
    task?: Itask[]
}