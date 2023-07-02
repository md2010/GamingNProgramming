import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { MatSelectModule } from "@angular/material/select";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { StudentModuleModule } from './student-module/student-module.module';

import { LoginComponent } from './Login/login.component';
import { RegisterComponent } from './Register/register.component';
import { AppHeaderComponent } from './app-header/app-header.component';
import { UserService } from './services/UserService';


@NgModule({
  declarations: [
    AppComponent,
    AppHeaderComponent
  ],
  imports: [
    MatSelectModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    LoginComponent,
    RegisterComponent,
    HttpClientModule,
    StudentModuleModule
  ],
  exports: [
    MatSelectModule,
    BrowserAnimationsModule,
    AppHeaderComponent
  ],
  providers: [UserService],
  bootstrap: [AppComponent]
})
export class AppModule { 


}
