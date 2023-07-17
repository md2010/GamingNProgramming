import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from 'src/app/services/UserService';
import { Router } from '@angular/router';

import { FindStudentsComponent } from './find-students/find-students.component';
import { MyStudentsComponent } from './my-students/my-students.component';
import { CreateMapComponent } from '../create-map/create-map.component';
import { MyMapsComponent } from '../my-maps/my-maps.component';

@Component({
  selector: 'app-professor-dashboard',
  templateUrl: './professor-dashboard.component.html',
  styleUrls: ['./professor-dashboard.component.css'],
  standalone: true,
  imports: [FindStudentsComponent, MyStudentsComponent, CommonModule, CreateMapComponent, MyMapsComponent]
})
export class ProfessorDashboardComponent {
  constructor (private userService: UserService, private router: Router) {}

  user = <User>{};
  loaded = false;
  showFindStudents = false;
  showMyStudents = true;
  showCreateMap = false;
  showMyMaps = false;

  ngOnInit() {    
    this.getPlayer().then(() => {
      this.loaded = true;
    })
  }

  getPlayer() {
    var promise = new Promise((resolve, reject) => {
      this.userService.getProfessorById(sessionStorage.getItem('userId'))
    .subscribe(
      (Response) => {
        if(Response.body) {
          this.user = Response.body;
          resolve('done');
        }
      },
      (error: any) => {
        console.log(error.error);
        reject();
      }
    )
    })
    return promise;
  }
 
  findStudents(){
    this.showFindStudents = true;
    this.showMyStudents = false;
    this.showCreateMap = false;
    this.showMyMaps = false;
  }

  myStudents(){  
    this.showMyStudents = true;
    this.showFindStudents = false;
    this.showCreateMap = false;
    this.showMyMaps = false;
  }

  createMap() {
    this.showMyStudents = false;
    this.showFindStudents = false;
    this.showCreateMap = true;
    this.showMyMaps = false;
  }

  myMaps() {
    this.showMyStudents = false;
    this.showFindStudents = false;
    this.showCreateMap = false;
    this.showMyMaps = true;
  }

}

interface User {
  username: string;
  students: Array<Student>;
}


interface Student {
  userId: string,
  username: string
}



