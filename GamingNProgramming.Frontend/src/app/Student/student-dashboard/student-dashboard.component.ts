import { Component } from '@angular/core';
import { UserService } from 'src/app/services/UserService';
import { AvatarModule } from '@coreui/angular';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute} from '@angular/router';
import { FindFriendsComponentComponent } from './find-friends-component/find-friends-component.component';
import { MyFriendsComponent } from './my-friends/my-friends.component';
import { HomeComponent } from '../home/home.component';

@Component({
  selector: 'app-student-dashboard',
  templateUrl: './student-dashboard.component.html',
  styleUrls: ['./student-dashboard.component.css'],
  imports: [AvatarModule, FindFriendsComponentComponent, CommonModule, MyFriendsComponent, HomeComponent],
  providers: [UserService],
  standalone: true
})
export class StudentDashboardComponent {

  constructor (private userService: UserService, private router: Router) {}

  user = <User>{};
  loaded = false;
  showFindFriends = false;
  showMyFriends = false;
  showHome = true;

  ngOnInit() {    
    this.getPlayer().then(() => {
      this.loaded = true;
    })
  }

  getPlayer() {
    var promise = new Promise((resolve, reject) => {
      this.userService.getPlayerById(localStorage.getItem('userId'))
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
}

interface Avatar {
  path: string;
}


