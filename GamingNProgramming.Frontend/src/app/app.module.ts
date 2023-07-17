import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { JwtModule } from '@auth0/angular-jwt';
import { AppComponent } from './app.component';
import { NuMonacoEditorModule } from '@ng-util/monaco-editor';

import { MatSelectModule } from "@angular/material/select";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';

import { LoginComponent } from './Login/login.component';
import { RegisterComponent } from './Register/register.component';
import { AppHeaderComponent } from './app-header/app-header.component';
import { UserService } from './services/UserService';
import { StudentDashboardComponent } from './Student/student-dashboard/student-dashboard.component';
import { LeaderboardComponent } from './Student/leaderboard/leaderboard.component';
import { HomeComponent } from './Student/home/home.component';
import { MapInfoComponent } from './Student/map-info/map-info.component';
import { TaskPlayComponent } from './Student/task-play/task-play.component';
import { ProfessorDashboardComponent } from './Professor/professor-dashboard/professor-dashboard.component';
import { CreateMapComponent } from './Professor/create-map/create-map.component';
import { FindStudentsComponent } from './Professor/professor-dashboard/find-students/find-students.component';
import { MyStudentsComponent } from './Professor/professor-dashboard/my-students/my-students.component';
import { MyMapsComponent } from './Professor/my-maps/my-maps.component';
import { DrawMapComponent } from './Professor/create-map/draw-map/draw-map.component';

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
    MapInfoComponent,
    RegisterComponent,
    StudentDashboardComponent,
    LeaderboardComponent,
    TaskPlayComponent,
    HttpClientModule,
    JwtModule,
    HomeComponent,
    RouterModule,
    NuMonacoEditorModule.forRoot(),
    ProfessorDashboardComponent,
    FindStudentsComponent,
    MyStudentsComponent,
    CreateMapComponent,
    MyMapsComponent,
    DrawMapComponent
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
