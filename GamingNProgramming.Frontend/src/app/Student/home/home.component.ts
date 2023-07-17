import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  standalone: true
})
export class HomeComponent {

constructor(private router: Router) {}

mapInfo() {
  this.router.navigate(['/map-info', 1]);
}
}
