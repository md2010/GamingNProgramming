import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from 'src/app/services/UserService';
import { FormsModule } from '@angular/forms';
import { AuthService } from 'src/app/services/AuthService';
import { PlayerTask } from 'src/app/classes/Classes';
import { GameService } from 'src/app/services/GameService';

@Component({
  selector: 'app-leaderboard',
  templateUrl: './leaderboard.component.html',
  styleUrls: ['./leaderboard.component.css'],
  standalone: true,
  providers: [UserService],
  imports: [CommonModule, FormsModule]
})
export class LeaderboardComponent {

  constructor(private userService: UserService, private authService: AuthService, private gameService : GameService) {}
  role!: string | null
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
  defaultMapId = ''

  ngOnInit() { 
    this.defaultMapId = localStorage.getItem('defaultMapId')!
    if(this.defaultMapId === null) {
      this.gameService.getDefaultMap()
      .subscribe(
        (Response)=> {
          if(Response.body) {
            this.defaultMapId = Response.body.id
          }
        }
      )
    }
    this.role = this.authService.getAuthorized().roleName;
    this.userId = this.authService.getAuthorized().userId; 
    if(this.role === 'Student') {
      this.allStudents = false;
      this.allFriends = true;
      this.allPlayers = false;  
      this.getAllFriends();
    } 
    else {
      var search = {
        sortOrder: 'desc',
        professorId: this.userId,
        includeProperties: 'playerTasks,Avatar'
      };
      this.getPlayers(search, 'students').then(() => {
        this.players = this.students;
        this.allStudents = true;
        this.allPlayers = false;
        this.allFriends = false;
       this.loaded = true;
      });  
    }

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
      var search = {sortOrder: 'desc', includeUser : true};
      this.userService.getPlayersFriends(search)
    .subscribe(
      (Response) => {
        if(Response.body) {
          this.friends = Response.body;
          this.friends.every(p => { p.avatar.path = '../' + p.avatar.path; });
          this.friends.forEach(p => { 
            p.badges = [];
            p.playerTasks.forEach(element => {
            if(element.badge) {
              p.badges.push({path: element.badge?.path!, isDefaultMap: element.mapId === this.defaultMapId ? true : false});
            }             
          })
          this.players = this.friends;
          });        
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
      sortOrder: 'desc', includeProperties: 'playerTasks,Avatar'
    };
    this.getPlayers(search, 'players').then(() => { 
      return true 
    });
    return false;
  }

  getAllStudents() : boolean {
    var search = {
      sortOrder: 'desc',
      professorId: this.role === 'Student' ? localStorage.getItem('professorId') : this.userId,
      includeProperties: 'playerTasks,Avatar'
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
            this.playersAll.forEach(p => { 
              p.badges = [];
              p.playerTasks.forEach(element => {
              if(element.badge) {
                p.badges.push({path: element.badge?.path!, isDefaultMap: element.mapId === this.defaultMapId ? true : false});
              }             
            })
            });
          }
          else {
            this.students = Response.body;
            this.students.every(p => { p.avatar.path = '../' + p.avatar.path; });
            this.students.forEach(p => { 
              p.badges = [];
              p.playerTasks.forEach(element => {
              if(element.badge && element.mapId !== this.defaultMapId) {
                p.badges.push({path: element.badge?.path!, isDefaultMap: false});
              }             
            })
            });          
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
  defaultPoints : number;
  points : number;
  xPs : number;
  timeConsumed: number;
  defaultTimeConsumed: number;
  badges: Array<BadgeREST>;
  playerTasks: Array<PlayerTask>;
}

interface BadgeREST {
  path: string;
  isDefaultMap: boolean;
}

