import { Component, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from 'src/app/services/UserService';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-find-friends-component',
  templateUrl: './find-friends-component.component.html',
  styleUrls: ['./find-friends-component.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule],
  providers: [UserService]
})
export class FindFriendsComponentComponent {

constructor(private userService: UserService) {}

  players: Player[] = [];
  loaded = false;
  searchName = '';

  ngOnInit() {    
    this.getNotFriends().then(() => { 
      this.loaded = true 
    });    
  }

  getNotFriends() {
    var promise = new Promise((resolve, reject) => {
      this.userService.getPlayersNotFriends('')
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
      var search = { username : this.searchName};
      this.userService.getPlayersNotFriends(search)
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

  addFriend(id: string) {
    this.userService.addFriend(id)
    .subscribe(
      (Response) => {
        if(Response) {
          this.getNotFriends();
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
