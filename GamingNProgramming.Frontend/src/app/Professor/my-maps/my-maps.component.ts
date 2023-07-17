import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-my-maps',
  templateUrl: './my-maps.component.html',
  styleUrls: ['./my-maps.component.css'],
  standalone: true
})
export class MyMapsComponent {

  constructor(private router: Router){}

  mapInfo() {
    this.router.navigate(['/map-info', 1]);
  }
}
