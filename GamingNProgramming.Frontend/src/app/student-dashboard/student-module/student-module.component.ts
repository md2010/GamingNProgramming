import { Component } from '@angular/core';
import { UserService } from 'src/app/services/UserService';

@Component({
  selector: 'app-student-module',
  templateUrl: './student-module.component.html',
  styleUrls: ['./student-module.component.css'],
  providers: [UserService]
})
export class StudentModuleComponent {

  constructor (private userService: UserService) {}

  user = {};

  ngOnInit() {    
    this.userService.getPlayerById(localStorage.getItem('userId'))
    .subscribe(
      (Response) => {
        if(Response.body) {
          this.user = Response.body;
        }
      },
      (error: any) => {
        console.log(error.error);
      }
    )
  }
}

interface User {

}


