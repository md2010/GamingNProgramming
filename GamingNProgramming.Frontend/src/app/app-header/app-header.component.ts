import { Component } from '@angular/core';
import { UserService } from '../services/UserService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-app-header',
  templateUrl: './app-header.component.html',
  styleUrls: ['./app-header.component.css'],
  providers: [UserService]
})

export class AppHeaderComponent {

  authorized = localStorage.getItem('token') != null;
  role = localStorage.getItem('role');

  constructor(private userService: UserService, private router: Router) {}

  logout() {
    this.userService.logout();
    this.router.navigate(['/']);
  }

}
