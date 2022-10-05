import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { TokenService } from '../token.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  public logged: Subject<boolean> = new Subject<boolean>();

  constructor(private auth: TokenService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      
      const autenticated = this.auth.isAuth();

      if(autenticated)
      {
        this.logged.next(true);
        return true;
      }

      this.router.navigate(['/login']);
    
      return false;
  }
  
}
