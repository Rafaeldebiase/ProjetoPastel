import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Role } from 'src/app/shared/role';
import { RoutesHttpService } from 'src/app/routes-http.service';
import { IManager } from '../../shared/manager';
import { IPhone } from '../../shared/phone';
import { PhoneType } from '../../shared/phoneType';
import { IRegistration } from '../../shared/registration';
import { IRole } from 'src/app/shared/iRole';



@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  public form!: FormGroup;

  public registration!: IRegistration;
  public phone!: IPhone;
  public landlineType: PhoneType = PhoneType.LANDLINE;
  public mobileType: PhoneType = PhoneType.MOBILE;
  public roles!: IRole[];
  public managers!: IManager[];

  public managerVisible: boolean = false;

  localUrl: any;
  file!: File;

  constructor(
    private _fb: FormBuilder,
    private _http: RoutesHttpService,
    private _router: Router,
    private _snack: MatSnackBar
  ) {
    this.registration = {} as IRegistration;
    this.managers = [{} as IManager];
    this.roles = [{} as IRole];
    this.phone = {} as IPhone;

  }

  ngOnInit(): void {
    this.form = this._fb.group({
      firstName: new FormControl(this.registration.firstName, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      lastName: new FormControl(this.registration.lastName, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      email: new FormControl(this.registration.email, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250),
        Validators.email
      ]),
      password: new FormControl(this.registration.password, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      street: new FormControl(this.registration.street, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      streetNumber: new FormControl(this.registration.streetNumber, [
        Validators.required
      ]),
      streetComplement: new FormControl(this.registration.streetComplement),
      neighborhood: new FormControl(this.registration.neighborhood, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      city: new FormControl(this.registration.city, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      state: new FormControl(this.registration.state, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      contry: new FormControl(this.registration.country, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      zipCode: new FormControl(this.registration.zipCode, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      birthDate: new FormControl(this.registration.birthDate, [
        Validators.required
      ]),
      role: new FormControl(this.registration.role, [
        Validators.required
      ]),
      managerId: new FormControl(this.registration.managerId),
      phones: this._fb.array([])
    });

    this._setRole();
    this._setManager();
  }

  public newPhones(): FormGroup {
    return this._fb.group({
      number: [this.phone.number],
      type: [this.phone.type]
    });
  }

  get firstName() {
    return this.form.get('firstName')!;
  }

  get lastName() {
    return this.form.get('lastName')!;
  }

  get email() {
    return this.form.get('email')!;
  }

  get password() {
    return this.form.get('password')!;
  }

  get street() {
    return this.form.get('street')!;
  }

  get streetNumber() {
    return this.form.get('streetNumber')!;
  }

  get neighborhood() {
    return this.form.get('neighborhood')!;
  }

  get city() {
    return this.form.get('city')!;
  }

  get state() {
    return this.form.get('state')!;
  }

  get contry() {
    return this.form.get('contry')!;
  }

  get zipCode() {
    return this.form.get('zipCode')!;
  }

  get birthDate() {
    return this.form.get('birthDate')!;
  }

  get phones(): FormArray {
    return this.form.controls["phones"] as FormArray;
  }

  get role() {
    return this.form.get('role')!;
  }

  get managerId() {
    return this.form.get('managerId')!;
  }

  public addPhone() {
    this.phones.push(this.newPhones());
  }

  public deletePhone(index: number) {
    this.phones.removeAt(index);
  }

  public checkRole(): void {
    const option = this.form.controls["role"].value

    if (option === Role.USER) {
      this.managerVisible = true;
    }
    else {
      this.managerVisible = false;
    }
  }

  public register(): void {
    if (this.form.invalid) {
      for (const control of Object.keys(this.form.controls)) {
        if (this.form.controls[control].invalid) {
          this.form.controls[control].markAsTouched();
        }
      }
      return;
    }

    this.registration = this.form.value;

    this._http.addRegistration(this.registration).subscribe({
      next: data => {
        if (this.file !== undefined) {
          this._http.upload(
            this.file,
            data.user.id,
          ).subscribe(response => {
            this._snack.open('Cadastro realizado com sucesso', '', { panelClass: ["snack-sucess"] })
          })
        } else {
          this._snack.open('Cadastro realizado com sucesso', '', { panelClass: ["snack-sucess"] })
        }

      },
      error: err => {
        console.log(err)
        this._snack.open('Cadastro não realizado', '', { panelClass: ["snack-error"] })
      }
    })
    this.clearForm();
  }

  private _setManager(): void {
    this._http.getManagers().subscribe(resp => {
      this.managers = resp
    })
  }

  private _setRole(): void {
    const user: IRole = {
      id: Role.USER,
      name: "Usuário"
    }

    const manager: IRole = {
      id: Role.MANAGER,
      name: "Gestor"
    }

    this.roles = [user, manager];
    console.log(this.roles[0].id)
  }

  public clearForm(): void {
    this.form.reset();
  }

  public cancel(): void {
    this.form.reset();
    this._router.navigate(['/home']);
  }

  public selectFile(event: any) {
    this.file = <File>event.target.files[0];
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();
      reader.onload = (event: any) => {
        this.localUrl = event.target.result;
      }
      reader.readAsDataURL(event.target.files[0]);
    }
  }

}
