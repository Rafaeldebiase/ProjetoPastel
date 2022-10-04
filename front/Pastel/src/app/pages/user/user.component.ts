import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TokenService } from 'src/app/token.service';
import { DatePipe } from '@angular/common';
import { IUser } from 'src/app/user';
import { IUserTask } from 'src/app/shared/userTask';
import { IRegistration } from 'src/app/shared/registration';
import { IRole } from 'src/app/shared/iRole';
import { IManager } from 'src/app/shared/manager';
import { PhoneType } from 'src/app/shared/phoneType';
import { IPhone } from 'src/app/shared/phone';
import { Itask } from 'src/app/shared/task';
import { Role } from 'src/app/shared/role';
import { RoutesHttpService } from 'src/app/routes-http.service';
import { IDelete } from 'src/app/shared/delete';



declare var window: any;

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {

  public taskModal: any;
  public userModal: any;
  public phoneModal: any;

  public users: IUserTask[] = [];
  public panelOpenState = false;
  public hasTask: boolean = false;
  public edit: boolean = false;
  public registration!: IRegistration;
  public managerVisible: boolean = false
  public photo!: boolean;
  public userPhoto!: boolean;
  public roles!: IRole[];
  public formUser!: FormGroup;
  public formTask!: FormGroup;
  public formPhone!: FormGroup;
  public newTask!: Itask;
  public managers!: IManager[];
  public landlineType: PhoneType = PhoneType.LANDLINE;
  public mobileType: PhoneType = PhoneType.MOBILE;
  public localUrl: any;
  public file!: File;
  public phonesUser!: IPhone[];


  private _user!: IUser;
  private _phone!: IPhone;
  private _task!: Itask;
  private _role!: string;
  private _idManager!: string;
  private _idUser: string = '';

  constructor(
    private _token: TokenService,
    private _http: RoutesHttpService,
    private _fb: FormBuilder,
    private _snack: MatSnackBar,
    private _datePipe: DatePipe
  ) {
    this.newTask = {} as Itask;
    this.registration = {} as IRegistration;
    this.roles = [{} as IRole];
    this._phone = {} as IPhone;

  }

  ngOnInit(): void {
    this.taskModal = new window.bootstrap.Modal(
      document.getElementById('addtaskmodal')
    );

    this.userModal = new window.bootstrap.Modal(
      document.getElementById('usermodal')
    );

    this.phoneModal = new window.bootstrap.Modal(
      document.getElementById('phonemodal')
    );

    this.formPhone = this._fb.group({
      phones: this._fb.array([])
    });

    this.formTask = this._fb.group({
      message: new FormControl(this.newTask.message, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      deadline: new FormControl(this.newTask.deadline, [
        Validators.required
      ])
    });

    this.formUser = this._fb.group({
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
    });

    this._role = this._token.getRole();
    this._idManager = this._token.getId();
    this._getUsers();
    this._setManager();
    this._setRole();
  }

  public newPhones(): FormGroup {
    return this._fb.group({
      number: [this._phone.number],
      type: [this._phone.type]
    });
  }

  get phones(): FormArray {
    return this.formPhone.controls["phones"] as FormArray;
  }

  get firstName() {
    return this.formUser.get('firstName')!;
  }

  get lastName() {
    return this.formUser.get('lastName')!;
  }

  get email() {
    return this.formUser.get('email')!;
  }

  get password() {
    return this.formUser.get('password')!;
  }

  get street() {
    return this.formUser.get('street')!;
  }

  get streetNumber() {
    return this.formUser.get('streetNumber')!;
  }

  get neighborhood() {
    return this.formUser.get('neighborhood')!;
  }

  get city() {
    return this.formUser.get('city')!;
  }

  get state() {
    return this.formUser.get('state')!;
  }

  get contry() {
    return this.formUser.get('contry')!;
  }

  get zipCode() {
    return this.formUser.get('zipCode')!;
  }

  get birthDate() {
    return this.formUser.get('birthDate')!;
  }

  get role() {
    return this.formUser.get('role')!;
  }

  get managerId() {
    return this.formUser.get('managerId')!;
  }

  get message() {
    return this.formTask.get('message')!;
  }

  get deadline() {
    return this.formTask.get('deadline')!;
  }

  public addPhone() {
    this.phones.push(this.newPhones());
  }

  public deletePhone(index: number) {
    this.phones.removeAt(index);
  }

  private _getUsers(): void {
    if (this._role === Role.MANAGER) {
      this._http.getUsers(this._idManager).subscribe({
        next: data => {
          console.log(data);
          data.map(x => {
            console.log(x.userDto);
            this.users.push(x);
            if (x.task != undefined) {
              if (x.task.length > 0) {
                this.hasTask = true
              }
            }
          });
        },
        error: err => {
          console.log(err);
        }
      })
    } else {

    }
  }

  public saveTask(): void {
    if (this.formTask.invalid) {
      for (const control of Object.keys(this.formTask.controls)) {
        if (this.formTask.controls[control].invalid) {
          this.formTask.controls[control].markAsTouched();
        }
      }
      return;
    }

    this.newTask = this.formTask.value;
    this.newTask.userId = this._idUser;

    this._http.addTask(this.newTask).subscribe({
      next: data => {
        if (data.obj != undefined) {
          this._snack.open('Tarefa criada com sucesso', '', { panelClass: ["snack-sucess"] })
          this.users.forEach(user => {
            if (user.userDto.id === this._idUser) {
              user.task?.push(data.obj)
            }
          })
        }

      },
      error: err => {
        this._snack.open('Não foi possível criar a tarefa', '', { panelClass: ["snack-error"] })
      }
    })

    this.closeTaskModal();
  }

  public deleteTask(index: number, userId: string): void {
    this.users.forEach(user => {
      if (user.userDto.id === userId) {
        user.task?.forEach(task => {
          const deleteTask: IDelete = {
            id: task.id
          }
          this._http.deleteTask(deleteTask).subscribe({
            next: data => {
              if (data.erros != undefined) {
                if (data.erros.length > 0) {
                  data.erros.forEach(x =>
                    this._snack.open(x, '', { panelClass: ["snack-error"] })

                  );
                }
              }

              if (data.obj != undefined) {
                user?.task?.splice(index, 1);
                this._snack.open('Tarefa excluida com sucesso', '', { panelClass: ["snack-sucess"] })
              }
            },
            error: err => {
              this._snack.open('Não foi possível excluir a tarefa', '', { panelClass: ["snack-error"] })
            }
          })
        })
      }
    })
  }

  public editTask(): void {
    if (this.formTask.invalid) {
      for (const control of Object.keys(this.formTask.controls)) {
        if (this.formTask.controls[control].invalid) {
          this.formTask.controls[control].markAsTouched();
        }
      }
      return;
    }

    this.newTask = this.formTask.value;
    this.newTask.userId = this._task.userId;
    this.newTask.id = this._task.id;
    this.newTask.completed = false;

    this._http.editTask(this.newTask).subscribe({
      next: data => {
        if (data.obj != undefined) {
          this._snack.open('Tarefa criada com sucesso', '', { panelClass: ["snack-sucess"] })
          this.users.forEach(user => {
            if (user.userDto.id === this._task.userId) {
              user.task?.push(data.obj)
            }
          })
        }

        if (data.erros != undefined) {
          data.erros.forEach(x => this._snack.open(x, '', { panelClass: ["snack-error"] }))
        }
      },
      error: err => {
        this._snack.open('Não foi possível criar a tarefa', '', { panelClass: ["snack-error"] })
      }
    });
    this.closeTaskModal();
  }

  public editUser(): void {
    if (this.formUser.invalid) {
      for (const control of Object.keys(this.formUser.controls)) {
        if (this.formUser.controls[control].invalid) {
          this.formUser.controls[control].markAsTouched();
        }
      }
      return;
    }

    this.registration = this.formUser.value;
    this.registration.id = this._user.id;

    this._http.editUser(this.registration).subscribe({
      next: data => {
        if (this.file !== undefined) {
          this._http.upload(
            this.file,
            this._user.id,
          ).subscribe(response => {
            this._snack.open('Usuario editado com sucesso', '', { panelClass: ["snack-sucess"] })
          })
        } else {
          this._snack.open('Usuario editado com sucesso', '', { panelClass: ["snack-sucess"] })
        }
        this.closeUserModal();
      },
      error: err => {
        console.log(err)
        this._snack.open('Cadastro não realizado', '', { panelClass: ["snack-error"] })
        this.closeUserModal();
      }
    })
  }

  private _getPhoto(): void {
    this._http.getPhoto(this._user.id).subscribe({
      next: data => {
        console.log(data)
        if (data && data.size > 0) {
          this.photo = true;
          this.userPhoto = true;
          let reader = new FileReader();

          reader.onload = (event: any) => {
            this.localUrl = event.target.result;
          }
          reader.readAsDataURL(data)
        } else {
          this.photo = false;
          this.userPhoto = false;
        }
      },
      error: err => {
        console.log(err)
        this.photo = false;
      }
    })
  }

  public selectFile(event: any) {
    this.file = <File>event.target.files[0];
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();
      reader.onload = (event: any) => {
        this.localUrl = event.target.result;
      }
      reader.readAsDataURL(event.target.files[0]);
      this.photo = true;
    }
  }

  public deletePhoto(): void {
    const deletePhoto: IDelete = {
      id: this._user.id
    }
    if (this.userPhoto) {
      this._http.deletePhoto(deletePhoto).subscribe({
        next: data => {
          this.localUrl = "";
          this.photo = false;
          this._snack.open('Foto removida com sucesso', '', { panelClass: ["snack-sucess"] })
        },
        error: err => {
          this._snack.open('Não foi possível remover a foto', '', { panelClass: ["snack-error"] })
        }
      })
    }
  }

  public deleteUser(index: number, userId: string): void {
    this.users.forEach(user => {
      if (user.userDto.id === userId) {
        const deleteUser: IDelete = {
          id: user.userDto.id
        }
        this._http.deleteUser(deleteUser).subscribe({
          next: data => {
            if (data.erros != undefined) {
              if (data.erros.length > 0) {
                data.erros.forEach(erro =>
                  this._snack.open(erro, '', { panelClass: ["snack-error"] })
                );
              }
            }

            if (data.obj != undefined) {
              this.users.splice(index, 1);
              this._snack.open('Tarefa excluida com sucesso', '', { panelClass: ["snack-sucess"] })
            }
          },
          error: err => {
            this._snack.open('Não foi possível excluir a tarefa', '', { panelClass: ["snack-error"] })
          }
        })
      }
    })
  }

  public getPhone(): any {
    this._http.getPhone(this._user.id).subscribe({
      next: data => {
        console.log(data)
      },
      error: err => {
        console.log(err)
      }
    })
  }

  openTaskModal(idUser: string): void {
    this.taskModal.show();
    this._idUser = idUser;
  }

  openEditTaskModal(task: Itask) {
    this.edit = true;
    this._task = task;
    const date = this._datePipe.transform(task.deadline, 'yyyy-MM-dd');
    this.formTask.controls['message'].setValue(task.message);
    this.formTask.controls['deadline'].setValue(date);

    this.taskModal.show();
  }

  public closeTaskModal(): void {
    this.formTask.reset();
    this.taskModal.hide();
  }

  public openUserModal(user: IUser): void {
    this._user = user;
    this.userModal.show();
    this._setValues();
  }

  public closeUserModal(): void {
    this.formUser.reset();
    this.localUrl = '';
    this.userModal.hide();
  }

  public openPhoneModal(): void {
    this.phoneModal.show();
    this._setValues();
  }

  public closePhoneModal(): void {
    this.formUser.reset();
    this.phoneModal.hide();
  }


  private _setValues(): void {
    console.log(this._user);

    this.formUser.controls['firstName'].setValue(this._user.firstName);
    this.formUser.controls['lastName'].setValue(this._user.lastName);
    this.formUser.controls['email'].setValue(this._user.email);
    this.formUser.controls['street'].setValue(this._user.street);
    this.formUser.controls['streetNumber'].setValue(this._user.streetNumber);
    this.formUser.controls['streetComplement'].setValue(this._user.streetComplement);
    this.formUser.controls['neighborhood'].setValue(this._user.neighborhood);
    this.formUser.controls['city'].setValue(this._user.city);
    this.formUser.controls['state'].setValue(this._user.state);
    this.formUser.controls['contry'].setValue(this._user.contry);
    this.formUser.controls['zipCode'].setValue(this._user.zipCode);
    const date = this._datePipe.transform(this._user.birthDate, 'yyyy-MM-dd');
    this.formUser.controls['birthDate'].setValue(date);
    const role: Role = Role[this._user.role as keyof typeof Role];
    this.formUser.controls['role'].setValue(role);
    this.formUser.controls['managerId'].setValue(this._user.managerId);
    this.checkRole();
    this._getPhoto();
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
  }

  public checkRole(): void {
    const option = this.formUser.controls["role"].value

    if (option === Role.USER) {
      this.managerVisible = true;
    }
    else {
      this.managerVisible = false;
    }
  }

}
