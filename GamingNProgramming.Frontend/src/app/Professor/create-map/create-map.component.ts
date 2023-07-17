import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AvatarModule } from '@coreui/angular';

import { DrawMapComponent } from './draw-map/draw-map.component';

@Component({
  selector: 'app-create-map',
  templateUrl: './create-map.component.html',
  styleUrls: ['./create-map.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, DrawMapComponent, AvatarModule]
})
export class CreateMapComponent {

  levels : number = 1;
  tasks : number = 1;
  mapPath : string = ''

  constructor(private router: Router){}

  onMapSelected(value: string) {
    this.mapPath = value;
  }
}
