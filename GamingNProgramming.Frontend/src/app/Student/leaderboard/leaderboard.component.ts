import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from 'src/app/services/UserService';
import { FormsModule } from '@angular/forms';
import { AuthService } from 'src/app/services/AuthService';

@Component({
  selector: 'app-leaderboard',
  templateUrl: './leaderboard.component.html',
  styleUrls: ['./leaderboard.component.css'],
  standalone: true,
  providers: [UserService],
  imports: [CommonModule, FormsModule]
})
export class LeaderboardComponent {

  constructor(private userService: UserService, private authService: AuthService) {}

  friends: Player[] = [];
  playersAll: Player[] = [];
  players: Player[] = []
  students: Player[] = [];
  loaded = false;
  searchName = '';
  allPlayers = false;
  allStudents = false;
  allFriends = true;
  userId : string | null = '';

  ngOnInit() { 
    this.userId = this.authService.getAuthorized().userId; 
    this.allStudents = false;
    this.allFriends = true;
    this.allPlayers = false;  
    this.getAllFriends();
  }

  setPlayers(who : string) {
    this.loaded = false;
    if(who === 'students') {
      this.players = this.students;
      this.allStudents = true;
      this.allPlayers = false;
      this.allFriends = false;
    }
    else if(who === 'friends') {
      this.players = this.friends;
      this.allStudents = false;
      this.allFriends = true;
      this.allPlayers = false;
    }
    else {
      this.allFriends = false;
      this.allStudents = false;
      this.allPlayers = true;
      this.players = this.playersAll;
    }
    this.loaded = true;
  }

  getAllFriends() {
    this.getFriends().then(() => { 
      this.getAllPlayers();
      this.getAllStudents(); 
      this.loaded = true 
    }); 
  }

  getFriends() {
    var promise = new Promise((resolve, reject) => {
      var search = {sortOrder: 'asc', includeUser : true};
      this.userService.getPlayersFriends(search)
    .subscribe(
      (Response) => {
        if(Response.body) {
          this.friends = Response.body;
          this.friends.every(p => { p.avatar.path = '../' + p.avatar.path; });
          this.players = this.friends;
          resolve('done');
        }
      },
      (error: any) => {
        console.log(error.error);
        reject();
      }
    )});
    return promise;
  }

  getAllPlayers() : boolean {
    var search = {
      sortOrder: 'asc'
    };
    this.getPlayers(search, 'players').then(() => { 
      return true 
    });
    return false;
  }

  getAllStudents() : boolean {
    var search = {
      sortOrder: 'asc',
      professorId: localStorage.getItem('professorId')
    };
    if(search.professorId != null) {
      this.getPlayers(search, 'students').then(() => { 
        return true; 
      });
    }
    return false;
  }

  getPlayers(search : any, who : string) {
    var promise = new Promise((resolve, reject) => {     
      this.userService.getPlayers(search)
    .subscribe(
      (Response) => {
        if(Response.body) {
          if(who === 'players') {
            this.playersAll = Response.body;
            this.playersAll.every(p => { p.avatar.path = '../' + p.avatar.path; });
          }
          else {
            this.students = Response.body;
            this.students.every(p => { p.avatar.path = '../' + p.avatar.path; });
          }
          resolve('done');
        }
      },
      (error: any) => {
        console.log(error.error);
        reject();
      }
    )});
    return promise;
  }
 
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
  defultPoints : number;
}

