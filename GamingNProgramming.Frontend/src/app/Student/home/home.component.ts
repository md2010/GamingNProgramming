import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { GameService } from 'src/app/services/GameService';
import { Map } from 'src/app/classes/Classes';
import { CommonModule } from '@angular/common';
import { DrawMapComponent } from 'src/app/Professor/create-map/draw-map/draw-map.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  standalone: true,
  imports: [CommonModule, DrawMapComponent]
})
export class HomeComponent {

constructor(private router: Router, private gameService: GameService) {}

@Input() user: any 
maps : Array<Map> = []
map!: Map 
loaded : boolean = false;

ngOnInit() {
  if(this.user !== undefined) {
    var promise = new Promise((resolve, reject) => {
      if(this.user.professorId === null) 
        resolve('done')
      else {
      this.gameService.getMaps(this.user.professorId) 
        .subscribe(
          (Response) => {
            if(Response) {
              this.maps = Response.body; 
              resolve('done')      
            }        
          },
          (error: any) => {
            console.log(error.error);
            reject()
          }
        )
      }
    })
    promise.then(() => {
      this.gameService.getDefaultMap() 
      .subscribe(
        (Response) => {
          if(Response) {
            this.map = Response.body; 
            if(this.map !== null) {
              localStorage.setItem('defaultMapId', this.map!.id);
            }
            this.loaded = true;       
          }        
        },
        (error: any) => {
          console.log(error.error);
        }
      )
    })      
  }
}

mapInfo(id: string) {
  let map = this.maps.find(f => f.id === id);
  if(map === undefined) {
    map = this.map;
  }
  this.router.navigate(['/map-info',  map!.id], {state : { avatarSrc : this.user.avatar.path} });
}
}
