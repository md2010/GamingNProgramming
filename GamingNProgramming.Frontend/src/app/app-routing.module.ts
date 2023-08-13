import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Login/login.component';
import { RegisterComponent } from './Register/register.component';
import { StudentDashboardComponent } from './Student/student-dashboard/student-dashboard.component';
import { LeaderboardComponent } from './Student/leaderboard/leaderboard.component';
import { MapInfoComponent } from './Student/map-info/map-info.component';
import { TaskPlayComponent } from './Student/task-play/task-play.component';
import { TaskViewComponent } from './Student/task-view/task-view.component';

import { ProfessorDashboardComponent } from './Professor/professor-dashboard/professor-dashboard.component';
import { ReviewComponent } from './Professor/review/review.component';

const routes: Routes = [
  {path: 'login', component: LoginComponent },
  {path: 'register', component: RegisterComponent},
  {path: 'student-dashboard', component: StudentDashboardComponent}, 
  {path: 'leaderboard', component: LeaderboardComponent},
  {path: 'map-info/:id', component: MapInfoComponent},
  {path: 'task-play/:id', component: TaskPlayComponent},
  {path: 'task-view/:id', component: TaskViewComponent},
  {path: 'professor-dashboard', component: ProfessorDashboardComponent},
  {path: 'review/:id', component: ReviewComponent}
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {scrollPositionRestoration: 'enabled', useHash: true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
