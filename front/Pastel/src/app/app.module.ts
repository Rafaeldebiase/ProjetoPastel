import { NgModule } from '@angular/core';
import {DatePipe} from '@angular/common';
import { MatNativeDateModule } from '@angular/material/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { CommonModule } from '@angular/common';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { RegistrationComponent } from './pages/registration/registration.component';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { MatMenuModule } from "@angular/material/menu";
import { MatListModule } from "@angular/material/list";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatSnackBarModule, MAT_SNACK_BAR_DEFAULT_OPTIONS } from "@angular/material/snack-bar";
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

import { AuthService } from './auth.service';
import { TokenService } from './token.service';
import { RoutesHttpService } from './routes-http.service';
import { authInterceptorProviders } from './shared/auth.interceptor';
import { UserComponent } from './pages/user/user.component';
import { TaskManagerComponent } from './pages/task-manager/task-manager.component';
import { AuthGuard } from './Guards/auth.guard';
import { AboutComponent } from './pages/about/about.component';




@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    RegistrationComponent,
    UserComponent,
    TaskManagerComponent,
    AboutComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatToolbarModule,
    MatIconModule,
    MatMenuModule,
    MatListModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatTooltipModule,
    MatSnackBarModule,
    MatExpansionModule,
    CommonModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatNativeDateModule,
    MatInputModule,
    MatSlideToggleModule
  ],
  providers: [
    AuthService, 
    TokenService,
    RoutesHttpService,
    authInterceptorProviders,
    {provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: {duration: 2500}},
    DatePipe,
    AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
