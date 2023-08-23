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
import { CreateTaskDialogComponent } from './Professor/create-map/create-task-dialog/create-task-dialog.component';
import { TimerComponent } from './Student/task-play/timer/timer.component';
import { TaskViewComponent } from './Student/task-view/task-view.component';
import { ReviewComponent } from './Professor/review/review.component';
import { BattleComponent } from './Student/battle/battle.component';
import { PlayComponent } from './Student/battle/play/play.component';
import { MyBattlesComponent } from './Student/student-dashboard/my-battles/my-battles.component';
import { RulesComponent } from './rules/rules.component';

@NgModule({
  declarations: [
    AppComponent,
    AppHeaderComponent
  ],
  imports: [
    MatSelectModule,
    BrowserModule,
    BrowserAnimationsModule,
    LoginComponent,
    MapInfoComponent,
    RegisterComponent,
    StudentDashboardComponent,
    LeaderboardComponent,
    TaskPlayComponent,
    HttpClientModule,
    CreateTaskDialogComponent,
    JwtModule,
    HomeComponent,
    RouterModule,
    NuMonacoEditorModule.forRoot(),
    ProfessorDashboardComponent,
    FindStudentsComponent,
    MyStudentsComponent,
    CreateMapComponent,
    MyMapsComponent,
    DrawMapComponent,
    TimerComponent,
    TaskViewComponent,
    ReviewComponent,
    BattleComponent,
    PlayComponent,
    MyBattlesComponent,
    RulesComponent,
    AppRoutingModule
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
