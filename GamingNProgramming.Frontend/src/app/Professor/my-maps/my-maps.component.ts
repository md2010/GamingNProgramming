import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GameService } from 'src/app/services/GameService';
import { AuthService } from 'src/app/services/AuthService';
import { Map } from 'src/app/classes/Classes';
import { CommonModule } from '@angular/common';
import { DrawMapComponent } from '../create-map/draw-map/draw-map.component';

@Component({
  selector: 'app-my-maps',
  templateUrl: './my-maps.component.html',
  styleUrls: ['./my-maps.component.css'],
  standalone: true,
  imports: [CommonModule, DrawMapComponent]
})
export class MyMapsComponent {

  maps! : Array<Map>;
  loaded = false;

  constructor(private router: Router, private gameService: GameService, private authService: AuthService){}
  
  ngOnInit() {
    this.gameService.getMaps(this.authService.getAuthorized().userId!) 
    .subscribe(
      (Response) => {
        if(Response) {
          this.maps = Response.body;   
          this.loaded = true;      
        }        
      },
      (error: any) => {
        console.log(error.error);
      }
    )} 

  mapInfo(id: string) {
    let map = this.maps.find(f => f.id === id);
    this.router.navigate(['/map-info',  map!.id], {state : { map : map} });
  }
}
