import { Component } from '@angular/core';
import { AuthService } from '../services/AuthService';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-app-header',
  templateUrl: './app-header.component.html',
  styleUrls: ['./app-header.component.css'],
  providers: [AuthService]
})

export class AppHeaderComponent {

  authorized! : boolean | null;
  authorizedSub! : Subscription;

  role : string | null= "";
  route = "";

  constructor(private authService: AuthService, private activeRoute: ActivatedRoute, private router: Router) {
    this.authorizedSub = this.authService.authorized.subscribe(data => {
      this.authorized = data.isAuth
      this.role = data.roleName
    });
  }

  ngOnInit() {  
    this.activeRoute.url.subscribe((event) => { 
      this.route = event[0].path;
    });
  }

  home() {
    if(this.authorized && this.role === 'Student') {
      return '/student-dashboard';
    }
    return '/professor-dashboard';
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['login']);;
  }

  ngOnDestory(): void {
    this.authorizedSub.unsubscribe(); 
  }

}
interface Authorized {
  isAuth : boolean
}
