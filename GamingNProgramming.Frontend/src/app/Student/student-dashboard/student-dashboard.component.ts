import { Component } from '@angular/core';
import { UserService } from 'src/app/services/UserService';
import { AvatarModule } from '@coreui/angular';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute} from '@angular/router';
import { FindFriendsComponentComponent } from './find-friends-component/find-friends-component.component';
import { MyFriendsComponent } from './my-friends/my-friends.component';
import { HomeComponent } from '../home/home.component';
import { AuthService } from 'src/app/services/AuthService';

@Component({
  selector: 'app-student-dashboard',
  templateUrl: './student-dashboard.component.html',
  styleUrls: ['./student-dashboard.component.css'],
  imports: [AvatarModule, FindFriendsComponentComponent, CommonModule, MyFriendsComponent, HomeComponent],
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
          this.usersPointsOnProfessorMaps = this.user.points;
          this.professorMapPoints = Response.body.sum;
          if (this.professorMapPoints > 0 && this.usersPointsOnProfessorMaps) {
            this.userPercentage = Math.round((this.usersPointsOnProfessorMaps/this.professorMapPoints)*100)
            this.calculateDegs();
          }
          this.usersPointsOnMaps = this.user.defultPoints;
          this.mapsPoints = Response.body.sum;
          if (this.mapsPoints > 0 && this.usersPointsOnMaps) {
            this.userDefaultPercentage = Math.round((this.usersPointsOnMaps/this.mapsPoints)*100)
            this.calculateDegs();
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

  calculateDegs() {
    if(this.usersPointsOnProfessorMaps <= this.professorMapPoints/2) {
      var deg1 = (this.usersPointsOnProfessorMaps / (this.professorMapPoints/2)) * 180;
      this.professorMapPointsDeg1 = deg1 + 'deg';
      this.professorMapPointsDeg2 = 0 + 'deg';
    }
    else if (this.usersPointsOnProfessorMaps >= this.professorMapPoints/2) {
      this.professorMapPointsDeg1 = 180 + 'deg';
      var deg2 = ((this.usersPointsOnProfessorMaps/2) / (this.professorMapPoints/2)) * 180;
      this.professorMapPointsDeg2 = deg2 + 'deg';
    }
    
  }
 
  findFriends(){
    this.showFindFriends = true;
    this.showHome = false;
    this.showMyFriends = false;
  }

  myFriends(){  
    this.showMyFriends = true;
    this.showHome = false;
    this.showFindFriends = false;
  }

  home() {
    this.showMyFriends = false;
    this.showHome = true;
    this.showFindFriends = false;
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


