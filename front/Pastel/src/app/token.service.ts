import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Role } from './shared/role';

const TOKEN_KEY = 'auth-token';
const USER_KEY = 'auth-user';
const ID_KEY = 'auth-user-id';
const ROLE_KEY = 'auth-user-role';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor() { }

  public signOut(): void {
    window.sessionStorage.clear();
  }

  public saveToken(token: string): void {
    window.sessionStorage.removeItem(TOKEN_KEY);
    window.sessionStorage.setItem(TOKEN_KEY, token);
  }

  public isAuth(): boolean {
    if (this.getToken()) {
      return true;
    } else {
      return false;
    }
  }

  public isManager(): boolean {
    const role = this.getRole();
    return role === Role.MANAGER
  }

  public getToken(): string | null {
    return window.sessionStorage.getItem(TOKEN_KEY);
  }

  public saveUser(user: any): void {
    window.sessionStorage.removeItem(USER_KEY);
    window.sessionStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  public getUser(): any {
    const user = window.sessionStorage.getItem(USER_KEY);
    if (user) {
      return JSON.parse(user);
    }

    return {};
  }

  public saveId(id: string): void {
    window.sessionStorage.removeItem(ID_KEY);
    window.sessionStorage.setItem(ID_KEY, id);
  }

  public getId(): any {
    return window.sessionStorage.getItem(ID_KEY);
  }

  public saveRole(role: string): void {
    window.sessionStorage.removeItem(ROLE_KEY);
    window.sessionStorage.setItem(ROLE_KEY, role);
  }

  public getRole(): any {
    return window.sessionStorage.getItem(ROLE_KEY);
  }
}
