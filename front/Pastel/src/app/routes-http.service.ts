import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IManager } from './pages/registration/manager';


const AUTH_API = 'https://localhost:7185/api/v1/';

@Injectable({
  providedIn: 'root'
})
export class RoutesHttpService {

  constructor(private http: HttpClient) { }

  public getManagers(): Observable<IManager[]> {
    return this.http.get<IManager[]>(AUTH_API + 'User/managers');
  }
}
