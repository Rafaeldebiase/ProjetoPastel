import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { AuthGuard } from './Guards/auth.guard';
import { Role } from './shared/role';
import { TokenService } from './token.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  public singIn: boolean = true;
  public singOut: boolean = false;
  public userName: string = "";
  public isManager: boolean = false;

  constructor(
    private router: Router,
    private auth: AuthGuard,
    private token: TokenService
  ) {

  }
  ngOnInit(): void {
    this.isLogged();
  }

  public isLogged(): void {
    this.auth.logged.subscribe(response => {
      this.singIn = !response;
      this.singOut = response;
      this.userName = this.token.getUser();
      this.isManager = this.token.isManager();

    });
  }

  public logout(): void {
    this.token.signOut();
    this.singIn = true;
    this.singOut = false;
    this.userName = "";
    this.router.navigate(['/login'])
  }

  public goToHome(): void {
    this.router.navigate(['/home']);
  }

  public goToLogin(): void {
    this.router.navigate(['/login']);
  }

  public goToRegistration(): void {
    this.router.navigate(['/registration']);
  }

  public goToUser(): void {
    this.router.navigate(['/user']);
  }

  public goToTask(): void {
    this.router.navigate(['/task']);
  }

  public goToAbout(): void {
    this.router.navigate(['/about']);
  }

}
