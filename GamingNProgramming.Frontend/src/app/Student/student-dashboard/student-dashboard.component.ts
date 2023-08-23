import { Component } from '@angular/core';
import { UserService } from 'src/app/services/UserService';
import { AvatarModule } from '@coreui/angular';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute} from '@angular/router';
import { FindFriendsComponentComponent } from './find-friends-component/find-friends-component.component';
import { MyFriendsComponent } from './my-friends/my-friends.component';
import { HomeComponent } from '../home/home.component';
import { AuthService } from 'src/app/services/AuthService';
import { MyBattlesComponent } from './my-battles/my-battles.component';
import { SpinnerComponentComponent } from 'src/app/spinner-component/spinner-component.component';

@Component({
  selector: 'app-student-dashboard',
  templateUrl: './student-dashboard.component.html',
  styleUrls: ['./student-dashboard.component.css'],
  imports: [AvatarModule, FindFriendsComponentComponent, CommonModule, MyFriendsComponent, HomeComponent, MyBattlesComponent, SpinnerComponentComponent],
  providers: [UserService],
  standalone: true
})
export class StudentDashboardComponent {

  constructor (private userService: UserService, private router: Router, private authService: AuthService) {}

  user : any | null = null;
  loaded = false;
  showFindFriends = false;
  showMyFriends = false;
  showHome = true;
  showMyBattles = false;

  professorMapPoints = 0;
  usersPointsOnProfessorMaps = 0;
  userPercentage = 0;
  professorMapPointsDeg1 = '0deg';
  professorMapPointsDeg2 = '0deg';

  mapsPoints = 0;
  usersPointsOnMaps = 0;
  userDefaultPercentage = 0;
  mapPointsDeg1 = '0deg';
  mapPointsDeg2 = '0deg';

  ngOnInit() {    
    this.getPlayer().then(() => {
      this.loaded = true;
    })
  }

  getPlayer() {
    var promise = new Promise((resolve, reject) => {
      this.userService.getPlayerById(this.authService.getAuthorized().userId!)
    .subscribe(
      (Response) => {
        if(Response.body) {
          this.user = Response.body.player;
          localStorage.setItem('professorId', this.user.professorId);
          localStorage.setItem('avatarSrc', this.user.avatar.path);
          localStorage.setItem('username', this.user.username);
          this.usersPointsOnProfessorMaps = this.user.points;
          this.professorMapPoints = Response.body.sum;
          if (this.usersPointsOnProfessorMaps > 0 && this.professorMapPoints > 0) {
            this.userPercentage = Math.round((this.usersPointsOnProfessorMaps/this.professorMapPoints)*100)
            this.calculateDegs(this.usersPointsOnProfessorMaps, this.professorMapPoints, false);
          }
          this.usersPointsOnMaps = this.user.defaultPoints;
          this.mapsPoints = 73
          if (this.mapsPoints > 0 && this.usersPointsOnMaps > 0) {
            this.userDefaultPercentage = Math.round((this.usersPointsOnMaps/this.mapsPoints)*100)
            this.calculateDegs(this.usersPointsOnMaps, this.mapsPoints, true);
          }
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

  calculateDegs(userPoints : number, mapPoints : number, defaultMap : boolean) {
    if(userPoints <= mapPoints/2) {
      var deg1 = (userPoints / (mapPoints/2)) * 180;
      if(!defaultMap) {
        this.professorMapPointsDeg1 = deg1 + 'deg';
        this.professorMapPointsDeg2 = 0 + 'deg';
      }
      else {
        this.mapPointsDeg1 = deg1 + 'deg';
        this.mapPointsDeg2 = 0 + 'deg';
      }
    }
    else if (userPoints >= mapPoints/2) {
      this.professorMapPointsDeg1 = 180 + 'deg';
      var deg2 = (userPoints/2/mapPoints/2) * 180;
      if(!defaultMap)
        this.professorMapPointsDeg2 = deg2 + 'deg';
      else 
        this.mapPointsDeg2 = deg2 + 'deg';
    }
    
  }
 
  findFriends(){
    this.showFindFriends = true;
    this.showHome = false;
    this.showMyFriends = false;
    this.showMyBattles = false;
  }

  myFriends(){  
    this.showMyFriends = true;
    this.showHome = false;
    this.showFindFriends = false;
    this.showMyBattles = false;
  }

  home() {
    this.showMyFriends = false;
    this.showHome = true;
    this.showFindFriends = false;
    this.showMyBattles = false;
  }

  myBattles() {
    this.showMyFriends = false;
    this.showHome = false;
    this.showFindFriends = false;
    this.showMyBattles = true;
  }

}

interface User {
  avatar: Avatar;
  username: string;
  professorId : string | null
}

interface Avatar {
  path: string;
}


