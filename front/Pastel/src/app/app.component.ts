import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { TokenService } from './token.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  public singIn: boolean = true;
  public singOut: boolean = true;
  public userName: string = "";
  
  constructor(private router: Router, private token: TokenService) {
    
  }
  ngOnInit(): void {
    // this.isLogged();
  }

  public isLogged(): void {
    this.token.logged.subscribe(response => {
      this.singIn = !response;
      this.singOut = response;
      this.userName = this.token.getUser();
    });
  }

  public logout(): void {
    this.token.signOut();
    this.singIn = true;
    this.singOut = false;
    this.userName = "";
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

  public goToTask(): void {
    this.router.navigate(['/task']);
  }

}
