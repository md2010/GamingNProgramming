import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from 'src/app/services/UserService';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-leaderboard',
  templateUrl: './leaderboard.component.html',
  styleUrls: ['./leaderboard.component.css'],
  standalone: true,
  providers: [UserService],
  imports: [CommonModule, FormsModule]
})
export class LeaderboardComponent {

  constructor(private userService: UserService) {}

  friends: Friend[] = [];
  players: Player[] = [];
  loaded = false;
  searchName = '';
  allPlayers = false;

  ngOnInit() {    
    this.getAllFriends();
  }

  getAllFriends() {
    this.allPlayers = false;
    this.loaded = false;
    this.getFriends().then(() => { 
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
          this.players = Response.body;
          this.players.every(p => { p.avatar.path = '../' + p.avatar.path; });
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

  getAllPlayers() {
    this.allPlayers = true;
    this.loaded = false;
    this.getPlayers().then(() => { 
      this.loaded = true 
    });
  }

  getPlayers() {
    var promise = new Promise((resolve, reject) => {
      var search = {
        sortOrder: 'asc'
      };
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
}

