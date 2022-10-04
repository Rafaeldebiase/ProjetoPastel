import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IManager } from './shared/manager';
import { IRegistration } from './shared/registration';
import { IDelete } from './shared/delete';
import { INewPhone } from './shared/newPhone';
import { IResultPhone } from './shared/resultPhone';
import { Itask } from './shared/task';
import { IUserPhone } from './shared/userPhone';
import { IUserTask } from './shared/userTask';
import { IResponse } from './shared/response';
import { IUserResponseDto } from './shared/userResponseDto';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class RoutesHttpService {

  private API = environment.url


  constructor(private http: HttpClient) { }

  public getManagers(): Observable<IManager[]> {
    return this.http.get<IManager[]>(this.API + 'User/managers');
  }

  public getUsers(managerId: string): Observable<IUserTask[]> {
    return this.http.get<IUserTask[]>(this.API + `User/getUsers?managerId=${managerId}`);
  }

  public addRegistration(data: IRegistration): Observable<IUserResponseDto> {
    return this.http.post<IUserResponseDto>(this.API + 'User/create', data, httpOptions);
  }

  public editUser(data: IRegistration): Observable<IResponse> {
    return this.http.put<IResponse>(this.API + 'User/edit', data, httpOptions);
  }

  public addTask(data: Itask): Observable<IResponse> {
    return this.http.post<IResponse>(this.API + 'Task/create', data, httpOptions);
  }

  public deleteTask(data: IDelete): Observable<IResponse> {
    return this.http.post<IResponse>(this.API + 'Task/delete', data, httpOptions);
  }

  public editTask(data: Itask): Observable<IResponse> {
    return this.http.put<IResponse>(this.API + 'Task/edit', data, httpOptions);
  }

  public upload(file: File, id: string): Observable<IResponse> {   
      const formData: FormData = new FormData();
      console.log(file.name);
      console.log(file);
      formData.append('image', file);
  
      return this.http.post<IResponse>(this.API + `User/uploadphoto?userId=${id}`, formData);
  }

  public getPhoto(id: string): Observable<Blob> {
    return this.http.get(this.API + `User/getphoto?userId=${id}`, { responseType: 'blob' });
  }

  public deletePhoto(data: IDelete): Observable<IResponse> {
    return this.http.post<IResponse>(this.API + 'User/deletephoto', data, httpOptions)
  }

  public deleteUser(data: IDelete): Observable<IResponse> {
    return this.http.post<IResponse>(this.API + 'User/delete', data, httpOptions)
  }

  public getPhone(userId: string): Observable<IResultPhone> {
    return this.http.get<IResultPhone>(this.API + `User/getphone?userId=${userId}`);
  }

  public addPhone(data: INewPhone): Observable<IResultPhone> {
    return this.http.post<IResultPhone>(this.API + 'User/addphone', data, httpOptions);
  }

  public deletePhone(data: IUserPhone): Observable<IResponse> {
    return this.http.post<IResponse>(this.API + 'User/removephone', data, httpOptions)
  }
}
