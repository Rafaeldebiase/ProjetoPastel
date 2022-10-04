import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IManager } from './pages/registration/manager';
import { IRegistration } from './pages/registration/registration';
import { IDelete } from './pages/user/delete';
import { Itask } from './pages/user/task';
import { IUserTask } from './pages/user/userTask';
import { IResponseDto } from './responseDto';
import { IUserResponseDto } from './userResponseDto';

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

  public postRegistration(data: IRegistration): Observable<IUserResponseDto> {
    return this.http.post<IUserResponseDto>(this.API + 'User/create', data, httpOptions);
  }

  public editUser(data: IRegistration): Observable<IResponseDto> {
    return this.http.put<IResponseDto>(this.API + 'User/edit', data, httpOptions);
  }

  public postTask(data: Itask): Observable<IResponseDto> {
    return this.http.post<IResponseDto>(this.API + 'Task/create', data, httpOptions);
  }

  public deleteTask(data: IDelete): Observable<IResponseDto> {
    return this.http.post<IResponseDto>(this.API + 'Task/delete', data, httpOptions);
  }

  public editTask(data: Itask): Observable<IResponseDto> {
    return this.http.put<IResponseDto>(this.API + 'Task/edit', data, httpOptions);
  }

  public upload(file: File, id: string): Observable<IResponseDto> {   
      const formData: FormData = new FormData();
      console.log(file.name);
      console.log(file);
      formData.append('image', file);
  
      return this.http.post<IResponseDto>(this.API + `User/uploadphoto?userId=${id}`, formData);
  }

  public getPhoto(id: string): Observable<Blob> {
    return this.http.get(this.API + `User/getphoto?userId=${id}`, { responseType: 'blob' });
  }

  public deletePhoto(data: IDelete): Observable<IResponseDto> {
    return this.http.post<IResponseDto>(this.API + 'User/deletephoto', data, httpOptions)
  }

  public deleteUser(data: IDelete): Observable<IResponseDto> {
    return this.http.post<IResponseDto>(this.API + 'User/delete', data, httpOptions)
  }

  public getPhone(userId: string): Observable<IResponseDto> {
    return this.http.get<IResponseDto>(this.API + `User/getphone?userId=${userId}`);

  }
}
