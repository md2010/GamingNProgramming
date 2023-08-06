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

@Component({
  selector: 'app-map-info',
  templateUrl: './map-info.component.html',
  styleUrls: ['./map-info.component.css'],
  standalone: true,
  imports: [CommonModule, CreateTaskDialogComponent, DrawMapComponent, SpinnerComponentComponent]
  
})
export class MapInfoComponent {

  id : string | null = null
  sub : any;
  mapId! : string | null;
  map! : Map
  role! : string | null
  userLevel : number | null = null
  userTask : number | null = null
  playersTasks : Array<PlayerTask> = []
  loaded : boolean = false
  avatarSrc : string = ''

  constructor( private route: ActivatedRoute, private router: Router, private authService : AuthService, public dialog: MatDialog, private gameService: GameService) { 
    if(this.router.getCurrentNavigation()!.extras!.state!) {
      this.avatarSrc = this.router.getCurrentNavigation()!.extras!.state!['avatarSrc'];
      localStorage.setItem('avatarSrc', this.avatarSrc);
    }
    else {
      this.avatarSrc = localStorage.getItem('avatarSrc')!;
    }

  }

  ngOnInit() {
    this.sub = this.route.paramMap.subscribe((params) => {
      console.log(params);
      this.mapId = params.get('id');
    });
    this.role = this.authService.getAuthorized().roleName
    this.getMap().then(() => {
      if(this.role == 'Student')
      {
        this.gameService.getPlayerTask(this.authService.getAuthorized().userId!) 
        .subscribe(
        (Response) => {
          if(Response) {
            this.playersTasks = Response.body            
            var task = this.playersTasks[0].assignment;     
            var level = this.map.levels.find(l => l.id === task.levelId);
            if(level) {
              this.userLevel = level!.number - 1;
              this.userTask = task.number;
            }
            else {
              this.userLevel = 0;
              this.userTask = 0;
            }
            this.loaded = true
          }
        (error: any) => {
          console.log(error.error);
        }
      })
    }
  })    
}  

  getMap() {
    var promise = new Promise((resolve, reject) => { this.gameService.getMap(this.mapId!) 
      .subscribe(
        (Response) => {
          if(Response) {
            this.map = Response.body;  
            this.loaded = this.authService.getAuthorized().roleName === 'Student' ? false : true 
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

  openDialog(i : number, levelNumber: number) {
    let dialogRef = this.dialog.open(CreateTaskDialogComponent, {
      width: '1500px',
      height: '900px', 
      data: { task: this.map.levels[levelNumber].assignments[i], index : i, disableEdit : true },    
    });
  }

  taskPlay(taskId : string, levelId : string) {
    this.router.navigate(['/task-play',  taskId], {state : { isDefaultMap : (this.map.professorId !== '' ? false : true), mapId: this.map.id} });
  }

  onBack(): void {
    if(this.role == 'Professor') {
      this.router.navigate(['professor-dashboard']);
    }
    else {
      this.router.navigate(['student-dashboard']);
    }
  }

  ngOnDestroy() {
    if (this.sub) this.sub.unsubscribe();
  }
}
