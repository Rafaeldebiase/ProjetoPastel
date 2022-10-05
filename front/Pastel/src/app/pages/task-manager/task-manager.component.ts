
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RoutesHttpService } from 'src/app/routes-http.service';
import { ICloseTask } from 'src/app/shared/closeTask';
import { IDelete } from 'src/app/shared/delete';
import { Itask } from 'src/app/shared/task';
import { TokenService } from 'src/app/token.service';

@Component({
  selector: 'app-task-manager',
  templateUrl: './task-manager.component.html',
  styleUrls: ['./task-manager.component.scss']
})
export class TaskManagerComponent implements OnInit {

  public formEditTask!: FormGroup;
  public formTask!: FormGroup;
  public taskModel!: Itask;
  public newTask!: Itask;
  public tasksUser!: Itask[];
  public panelOpenState = false;
  public taskDelete!: IDelete;
  public isManager: boolean = false;

  private _userId!: string;


  constructor(
    private fb: FormBuilder,
    private http: RoutesHttpService,
    private token: TokenService,
    private _snack: MatSnackBar,
  ) {
    this.taskModel = {} as Itask;
    this.newTask = {} as Itask;
    this.taskDelete = {} as IDelete;
    this.tasksUser = [];
  }

  ngOnInit(): void {
    this.isManager = this.token.isManager();
    this._getTasks();

    this.formEditTask = this.fb.group({
      message: new FormControl(this.newTask.message, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      deadline: new FormControl(this.newTask.deadline, [
        Validators.required
      ])      
    })

    this.formTask = this.fb.group({
      message: new FormControl(this.newTask.message, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250)
      ]),
      deadline: new FormControl(this.newTask.deadline, [
        Validators.required
      ])
    });
  }

  get messageEdit() {
    return this.formTask.get('message')!;
  }

  get deadlineEdit() {
    return this.formTask.get('deadline')!;
  }

  get message() {
    return this.formTask.get('message')!;
  }

  get deadline() {
    return this.formTask.get('deadline')!;
  }

  private _getTasks(): void {
    this._userId = this.token.getId();

    this.http.getTasks(this._userId).subscribe({
      next: data => {
        this.tasksUser = [...data]
      },
      error: err => {
        console.log(err);
      }
    })
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
    this.newTask.userId = this._userId;

    this.http.addTask(this.newTask).subscribe({
      next: data => {
        if (data.obj) {
          this.tasksUser.push(data.obj)
          this._snack.open('Tarefa criada com sucesso', '', { panelClass: ["snack-sucess"] })
          this.clearTaskFrom();
        }
      },
      error: err => {
        this._snack.open('Não foi possível criar a tarefa', '', { panelClass: ["snack-error"] })
      }
    })
  }

  public deleteTask(index: number, task: Itask): void {
    this.taskDelete.id = task.id;

    this.http.deleteTask(this.taskDelete).subscribe({
      next: data => {
        if(data.obj) {
          this.tasksUser.splice(index, 1);
          this._snack.open('Tarefa excluida com sucesso', '', { panelClass: ["snack-sucess"] })
        }
      },
      error: err => {
        this._snack.open('Não foi possível excluir a tarefa', '', { panelClass: ["snack-error"] })
      }
    })
  }

  public ChangedStatus(task: Itask, event: any): void {
    const close: ICloseTask = {
      id: task.id,
      completed: event.checked
    }

    this.http.checkTask(close).subscribe({
      next: data => {
        if(data.obj){
          this._snack.open('Status da tarefa alterado com sucesso', '', { panelClass: ["snack-sucess"] })
        } else {
          data.erros.forEach(x => this._snack.open('x', '', { panelClass: ["snack-error"] }))
        }
      },
      error: err => {
        this._snack.open('Não foi possível alterar o status da tarefa', '', { panelClass: ["snack-error"] })
      }
    })
  }

  public clearTaskFrom(): void {
    this.formTask.reset();
  }
}

