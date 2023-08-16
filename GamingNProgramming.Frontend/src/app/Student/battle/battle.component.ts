import { Component, ElementRef, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AvatarModule } from '@coreui/angular';
import { AuthService } from 'src/app/services/AuthService';
import { GameService } from 'src/app/services/GameService';
import { ActivatedRoute, Router } from '@angular/router';
import { SpinnerComponent } from '@coreui/angular';
import { Battle } from 'src/app/classes/Classes';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import {MatCheckboxChange, MatCheckboxModule} from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { TimerComponent } from '../task-play/timer/timer.component';
import { PlayComponent } from './play/play.component';

@Component({
  selector: 'app-battle',
  templateUrl: './battle.component.html',
  styleUrls: ['./battle.component.css'],
  standalone: true,
  imports: [AvatarModule, 
    CommonModule, 
    SpinnerComponent,  
    MatCheckboxModule, 
    MatRadioModule, 
    TimerComponent, 
    MatDialogModule,
    PlayComponent
  ]
})
export class BattleComponent {

  sub : any;
  id : string | null = ''
  battle : Battle | null = null

  player1Avatar : string = ''
  player2Avatar : string = ''
  player1Username : string = ''
  player2Username : string = ''

  loaded : boolean = false;

  timerStarted = false;
  timerDone = false;
  currentTaskId : string = ''
  ids : Array<string> = []
  counter = 0;
  playing = false
  scoredPoints = 0
  usedTime = 0
  isLast = false

  @ViewChild('notification', { static: true }) notification!: TemplateRef<any>;

  constructor(private authService : AuthService, private gameService: GameService, private router : Router, private route: ActivatedRoute,
    public dialog: MatDialog) {
     this.player1Avatar = localStorage.getItem('avatarSrc')!;
     this.player1Username = localStorage.getItem('username')!;
     if(this.router.getCurrentNavigation()!.extras!.state!) {
      this.player2Avatar = this.router.getCurrentNavigation()!.extras!.state!['player2Avatar'];
      this.player2Username = this.router.getCurrentNavigation()!.extras!.state!['opponentUsername'];
    }
  }

  ngOnInit() {
    this.sub = this.route.paramMap.subscribe((params) => {
      console.log(params);
      this.id = params.get('id');
    });
    var promise = new Promise((resolve, reject) => {
    this.gameService.getBattle(this.id!)
    .subscribe(
      (Response) => {
          if(Response.status == 200) {
              this.battle = Response.body;
              this.ids = this.battle?.assignmentIds.split(',')!;
              resolve('done');
          }         
      },
      (error: any) => {
          console.log(error);
          reject();
      });   
    });
    promise.then(() => {
      this.loaded = true;
    })        
  }

  countDownDone(start: Date) {
    this.timerStarted = false;
    var now = new Date().getTime();
    this.usedTime = Math.floor((now-start.getTime()) / 1000 % 60) - 1;
    if(!this.timerDone) {
      this.timerDone = true;
      this.dialog.open(this.notification, { data: "Vrijeme je isteklo." });
    }
    this.updateBattle();
  }

  start() {
    this.currentTaskId = this.ids[this.counter]!;
    this.playing = true;
    this.timerStarted = true;
  }

  nextTask(points : number) {
    this.scoredPoints += points;
    this.counter++;
    this.currentTaskId = this.ids[this.counter]!;
    if(this.counter === this.ids.length - 1) {
      this.isLast = true;
    }  
  }

  endBattle(points : number) {
    this.scoredPoints += points;
    this.timerStarted = false;
    this.timerDone = true;
    this.playing = false;
  }

  updateBattle() {
    var data = {}
    if(this.battle?.player1Id === this.authService.getAuthorized().userId) {
      data = {
        id: this.battle.id,
        player1Time : this.usedTime,
        player1Points : this.scoredPoints
      }
    }
    else {
      data = {
        id: this.battle?.id,
        player2Time : this.usedTime,
        player2Points : this.scoredPoints
      }
    }
    
    this.gameService.updateBattle(data) 
    .subscribe(
      (Response) => {

      }
      ),
      (error : any) => {

      }
  }

}
