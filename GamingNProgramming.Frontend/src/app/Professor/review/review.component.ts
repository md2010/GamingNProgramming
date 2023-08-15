import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Assignment, Map, PlayerTask } from 'src/app/classes/Classes';
import { AuthService } from 'src/app/services/AuthService';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { CreateTaskDialogComponent } from 'src/app/Professor/create-map/create-task-dialog/create-task-dialog.component';
import { DrawMapComponent } from 'src/app/Professor/create-map/draw-map/draw-map.component';
import { GameService } from 'src/app/services/GameService';
import { SpinnerComponentComponent } from 'src/app/spinner-component/spinner-component.component';
import { TaskViewComponent } from 'src/app/Student/task-view/task-view.component';

@Component({
  selector: 'app-review',
  templateUrl: './review.component.html',
  styleUrls: ['./review.component.css'],
  standalone: true,
  imports: [CommonModule, CreateTaskDialogComponent, DrawMapComponent, SpinnerComponentComponent, TaskViewComponent]
  
})
export class ReviewComponent {

  id : string | null = null
  sub : any;
  maps! : Array<Map>
  role! : string | null
  userLevel : number | null = 0
  userTask : number | null = 0
  playersTasks : Array<PlayerTask> = []
  loaded : boolean = false
  playerId : string | null = null

  constructor( private route: ActivatedRoute, private router: Router, private authService : AuthService, public dialog: MatDialog, private gameService: GameService) { }

  ngOnInit() {
    this.sub = this.route.paramMap.subscribe((params) => {
      console.log(params);
      this.playerId = params.get('id');
      });
      this.role = this.authService.getAuthorized().roleName
      this.getMaps().then(() => {
        this.maps.forEach(map => {
          this.gameService.getPlayerTask(this.playerId!, map.id) 
          .subscribe(
          (Response) => {
            if(Response.body && Response.body.length > 0) {
              this.playersTasks = Response.body                                                
            }
          (error: any) => {
            console.log(error.error);
          }
        })
        });          
       this.loaded = true
    })    
  }
  
  getPercentage(taskId : string) {
    var playerTask = this.playersTasks.find(a => a.assignmentId === taskId);
    if(playerTask) {
      return playerTask.percentage;
    }
    return 0;
  }

  getMapPercentage(mapId : string, mapPoints: number) {
    var sum = 0;
    this.playersTasks.forEach(pt => {
      if(pt.mapId === mapId) {
        sum += pt.scoredPoints;
      }
    });
    return sum > 0 ? Math.round((sum / mapPoints)*100) : 0;
  }

  getMaps() {
    var promise = new Promise((resolve, reject) => { this.gameService.getMaps(this.authService.getAuthorized().userId!) 
      .subscribe(
        (Response) => {
          if(Response) {
            this.maps = Response.body;  
            this.loaded = true 
            resolve('done')     
          }        
        },
        (error: any) => {
          console.log(error.error);
          reject()
        }
    )
       });
     return promise;
  }

  isPlayed(taskId : string) {
    var playerTask = this.playersTasks.find(b => b.assignmentId === taskId)
    if(playerTask) {
      return true;
    } 
    else {
      return false;
    }                        
  }

  getColor(taskId : string) {
    if(this.isPlayed(taskId)) {
      var p = this.getPercentage(taskId);
      if(p! < 50) {
        return 'li-yellow';
      }
      else if (p! >= 50 ) {
        return 'li-green';
      }
    }
    return '';
  }

  taskView(taskId : string, mapId : string) {
    this.router.navigate(['/task-view',  taskId], {state : { isDefaultMap : false, mapId: mapId, playerId: this.playerId} });
  }

  onBack(): void {
    this.router.navigate(['professor-dashboard']);
  }

  ngOnDestroy() {
    if (this.sub) this.sub.unsubscribe();
  }
}
