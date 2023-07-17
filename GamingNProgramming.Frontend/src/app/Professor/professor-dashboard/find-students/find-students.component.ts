import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from 'src/app/services/UserService';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-find-students',
  templateUrl: './find-students.component.html',
  styleUrls: ['./find-students.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule]
})
export class FindStudentsComponent {
  constructor(private userService: UserService) {}

  players: Player[] = [];
  loaded = false;
  searchName = '';

  ngOnInit() {    
    this.find().then(() => { 
      this.loaded = true 
    });    
  }

  findStudents() {
    this.players = [];
    this.loaded = false;
    this.find().then(() => { 
      this.loaded = true;
    })  
  }

  find() {
    var promise = new Promise((resolve, reject) => {
      var search = { username : this.searchName};
      this.userService.getProfessorNotStudents(search)
    .subscribe(
      (Response) => {
        if(Response.body) {
          this.players = Response.body;
          this.players.every(p => { p.avatar.path = '../' + p.avatar.path; });
          resolve('done');
        }
      },
      (error: any) => {
        console.log(error.error);
        reject();
      }
    )
    });
     return promise;
  }

  addStudent(id: string) {
    this.userService.addStudent(id)
    .subscribe(
      (Response) => {
        if(Response) {
          this.find();
        }
      },
      (error: any) => {
        console.log(error.error);
      }
    )}   

}

interface Friend {
  userId: string;
  player1: Player;
  player2: Player;
}

interface Avatar {
  path: string;
}

interface Player {
  userId: string;
  username: string;
  avatar: Avatar;
}


