import { Component, Output, EventEmitter  } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from 'src/app/services/UserService';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-my-students',
  templateUrl: './my-students.component.html',
  styleUrls: ['./my-students.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class MyStudentsComponent {

  constructor(private userService: UserService) {}

  players: Player[] = [];
  loaded = false;
  searchName = '';
  @Output() reviewEvent = new EventEmitter<string>();

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
      var search = { username : this.searchName, professorId: sessionStorage.getItem("userId"), includeProperties: 'Avatar'};
      this.userService.getPlayers(search)
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

  review(value: any) {
    this.reviewEvent.emit(value);
  }

  deleteStudent(id: string) {
    this.userService.deleteStudent(id)
    .subscribe(
      (Response) => {
          if(Response.status == 200) {
              this.find();
          }         
      },
      (error: any) => {
          console.log(error);
      });       
  }
}


interface Avatar {
  path: string;
}

interface Player {
  userId: string;
  username: string;
  avatar: Avatar;
  points: number;
}

