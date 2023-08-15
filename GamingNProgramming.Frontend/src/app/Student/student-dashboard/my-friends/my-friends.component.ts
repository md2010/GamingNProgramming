import { Component, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from 'src/app/services/UserService';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { GameService } from 'src/app/services/GameService';
import { SpinnerComponentComponent } from 'src/app/spinner-component/spinner-component.component';

@Component({
  selector: 'app-my-friends',
  templateUrl: './my-friends.component.html',
  styleUrls: ['./my-friends.component.css'],
  providers: [UserService],
  standalone: true,
  imports: [CommonModule, FormsModule, SpinnerComponentComponent]
})
export class MyFriendsComponent {

  constructor(private userService: UserService, private router: Router, private gameService: GameService) {}

  friends: Friend[] = [];
  players: Player[] = [];
  loaded = false;
  searchName = '';
  battleId : string = ''

  ngOnInit() {    
    this.getFriends().then(() => { 
      this.loaded = true 
    });    
  }

  getFriends() {
    var promise = new Promise((resolve, reject) => {
      this.userService.getPlayersFriends('')
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

  findFriends() {
    this.players = [];
    this.loaded = false;
    this.find().then(() => { 
      this.loaded = true;
    })  
  }

  find() {
    var promise = new Promise((resolve, reject) => {
    if(this.searchName.length > 0) {
      var search = {username: this.searchName};
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
    )}
    });
     return promise;
  }

  deleteFriend(id: string) {
    this.userService.deleteFriend(id)
    .subscribe(
      (Response) => {
          if(Response.status == 200) {
              this.getFriends();
          }         
      },
      (error: any) => {
          console.log(error);
      });       
  }

  battle(opponentId : string, opponentAvatar: string) {
    var promise = new Promise((resolve, reject) => {
    this.gameService.startBattle(opponentId)
    .subscribe(
      (Response) => {
          if(Response.status == 200) {
              this.battleId = Response.body.id
              resolve('done');
          }         
      },
      (error: any) => {
          console.log(error);
          reject();
      });   
    });
    promise.then((id) => {
      this.router.navigate(['/battle', this.battleId], {state: {player2Avatar: opponentAvatar}});
    })    
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
