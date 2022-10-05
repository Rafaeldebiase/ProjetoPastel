import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth.service';
import { TokenService } from 'src/app/token.service';
import { ILogin } from '../../shared/ilogin';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public form!: FormGroup;
  public user!: ILogin;

  isLoggedIn = false;
  isLoginFailed = false;
  errorMessage = '';
  roles: string[] = [];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private auth: AuthService,
    private token: TokenService
  ) {
    this.user = {} as ILogin;
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      email: new FormControl(this.user.email, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      password: new FormControl(this.user.password, [
        Validators.required,
        Validators.minLength(4)
      ]),
    });

    if (this.token.getToken()) {
      this.isLoggedIn = true;
      this.roles = this.token.getUser().roles;
    }
  }

  get email() {
    return this.form.get('email')!;
  }

  get password() {
    return this.form.get('password')!;
  }

  public singIn(): void {
    if (this.form.invalid) {
      for (const control of Object.keys(this.form.controls)) {
        this.form.controls[control].markAsTouched();
      }
      return;
    }

    this.user = this.form.value;

    this.auth.login(this.user.email, this.user.password).subscribe({
      next: data => {
        this.token.saveToken(data.token);
        this.token.saveUser(data.user);
        this.token.saveId(data.id);
        this.token.saveRole(data.role);

        this.isLoginFailed = false;
        this.isLoggedIn = true;
        this.roles = this.token.getUser().roles;
        this.cancel();
      },
      error: err => {
        this.errorMessage = err.error.message;
        this.isLoginFailed = true;
      }
    })
  }

  public cancel(): void {
    this.form.reset();
    this.router.navigate(['/home']);
  }


}
