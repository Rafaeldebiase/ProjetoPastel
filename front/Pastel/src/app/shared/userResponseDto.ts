import { IUser } from "./user";


export interface IUserResponseDto {
    user: IUser,
    erros: string[]
}