import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Battle } from 'src/app/classes/Classes';
import { AuthService } from 'src/app/services/AuthService';
import { GameService } from 'src/app/services/GameService';
import { CommonModule } from '@angular/common';
import { SpinnerComponentComponent } from 'src/app/spinner-component/spinner-component.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-my-battles',
  templateUrl: './my-battles.component.html',
  styleUrls: ['./my-battles.component.css'],
  standalone: true,
  imports : [
    CommonModule,
    SpinnerComponentComponent,
    MatDialogModule
  ]
})
export class MyBattlesComponent {

  loaded = false;
  battles : Array<Battle> = []
  userId : string = ''

  @ViewChild('notification', { static: true }) notification!: TemplateRef<any>;

  constructor(private authService : AuthService, private gameService : GameService, public dialog: MatDialog, public router: Router) {}

  ngOnInit() {
    this.userId = this.authService.getAuthorized().userId!;
    var promise = new Promise((resolve, reject) => {
      this.gameService.findBattles(this.userId)
      .subscribe(
        (Response) => {
        if(Response.body && Response.body.length > 0) {
          this.battles = Response.body;
        }
        resolve('done');
        this.loaded = true;
      },
      (error: any) => {
        console.log(error);
        reject();
    });    
    })

    promise.then(() => {
      
    })
  }

  getTime(seconds : number) {
    var d = new Date(0,0,0,0,0,0,0);
    d.setSeconds(seconds);
    return d;
  }

  result(id : string) {
    var battle = (this.battles.find(a => a.id === id));
    var message = "Protivnik još nije odigrao bitku."
    var points = 0;
    if(battle?.wonId !== null) {
      if(battle?.wonId === this.userId){
        message = "Pobijedio si!"
        points = battle.player1Id === battle.wonId ? battle.player1Points*2 : battle.player2Points*2
      }
      else {
        message = "Izgubio si 2XPs."
      }
    }
    this.dialog.open(this.notification, { data: { message: message, won : message === "Pobijedio si!" ? true : false, points :  points}});
  }

  battle(id : string) {
    var battle = this.battles.find(a => a.id === id);
    var opponentAvatar = battle?.player1Id === this.userId ? battle.player2.avatar.path : battle?.player1.avatar.path
    var opponentUsername = battle?.player1Id === this.userId ? battle.player2.username : battle?.player1.username
    this.router.navigate(['/battle', id], {state: {player2Avatar: opponentAvatar, opponentUsername : opponentUsername}});
  }
}
