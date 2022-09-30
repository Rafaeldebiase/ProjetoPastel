import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Role } from 'src/app/role';
import { RoutesHttpService } from 'src/app/routes-http.service';
import { IManager } from './manager';
import { IPhone } from './phone';
import { PhoneType } from './phoneType';
import { IRegistration } from './registration';
import { IRole } from './role';

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

  constructor(private fb: FormBuilder, private http: RoutesHttpService) {
    this.registration = {} as IRegistration;
    this.managers = [{} as IManager];
    this.roles = [{} as IRole];
    this.phone = {} as IPhone; 

  }

  ngOnInit(): void {
    this.form = this.fb.group({
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
      streetComplement: new FormControl(this.registration.streetComplement, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
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
      contry: new FormControl(this.registration.contry, [
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
      managerId: new FormControl(this.registration.managerId, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      phones: this.fb.array([])
    });

    this._setRole();
    this._setManager();
  }

  get phones(): FormArray {
    return this.form.controls["phones"] as FormArray;
  }

  public newPhones(): FormGroup {
    return this.fb.group({
      phoneNumber: [this.phone.number, Validators.required],
      phoneType: [this.phone.type, Validators.required]
    });
  }

  public addPhone() {
    this.phones.push(this.newPhones());
  }

  public deletePhone(index: number) {
    this.phones.removeAt(index);
  }

  public checkRole(): void {
    const option = this.form.controls["role"].value
    
    if(option === Role.USER)
    {
      this.managerVisible = true;
    }
  } 

  public register(): void {
    this.registration = this.form.value
  }

  private _setManager(): void {
    this.http.getManagers().subscribe(resp => {
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

}
