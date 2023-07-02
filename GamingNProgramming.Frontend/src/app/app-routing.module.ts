import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Login/login.component';
import { RegisterComponent } from './Register/register.component';
import { StudentModuleComponent } from './student-dashboard/student-module/student-module.component';

const routes: Routes = [
  {path: 'login', component: LoginComponent },
  {path: 'register', component: RegisterComponent},
  {path: 'student-dashboard', component: StudentModuleComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
