import { Component, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router'
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NuMonacoEditorModule, NuMonacoEditorModel } from '@ng-util/monaco-editor';
import { GameService } from 'src/app/services/GameService';
import { SpinnerComponentComponent } from 'src/app/spinner-component/spinner-component.component';
import { Assignment } from 'src/app/classes/Classes';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { AuthService } from 'src/app/services/AuthService';
import { TimerComponent } from './timer/timer.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-task-play',
  templateUrl: './task-play.component.html',
  styleUrls: ['./task-play.component.css'],
  standalone: true,
  imports: [
    FormsModule, 
    CommonModule, 
    NuMonacoEditorModule, 
    SpinnerComponentComponent, 
    MatCheckboxModule, 
    MatRadioModule, 
    TimerComponent, 
    MatDialogModule
  ]
})

export class TaskPlayComponent {

  id : string | null = null
  sub : any;
  compileResult : string | null = null
  error : boolean = true
  args : string | null = null
  loading = false;
  loaded = false;
  timerStarted = false;
  timerDone = false;
  checkedAnswers : Array<string> = [];
  checkedAnswer : string | null = null;
  theoryResult :  boolean = false
  correctAnswer : string | undefined = '';
  done :  boolean = false
  points : number = 0

  task! : Assignment;
  taskId! : string | null
  isDefaultMap! : boolean;
  mapId! : string;

  value: string = '';
  editorOptions = { theme: 'vs-dark', language: 'c' };
  model: NuMonacoEditorModel = {
    language: "c"
  }; 


  @ViewChild('notification', { static: true }) notification!: TemplateRef<any>;

  constructor( private route: ActivatedRoute, private router: Router, private gameService: GameService, private authService : AuthService, public dialog: MatDialog,) { 
    this.isDefaultMap = this.router.getCurrentNavigation()!.extras!.state!['isDefaultMap'];
    this.mapId = this.router.getCurrentNavigation()!.extras!.state!['mapId'];
   }

  ngOnInit() {
    this.sub = this.route.paramMap.subscribe((params) => {
      this.taskId = params.get('id');
    });
    this.gameService.getTask(this.taskId!) 
      .subscribe(
        (Response) => {
          if(Response) {
            this.task = Response.body;
            if (!this.task.isCoding)
              this.correctAnswer = this.task.answers.find((a) => a.isCorrect === true)?.offeredAnswer;
            else
            this.value = this.task.initialCode;        
          }
          this.loaded = true;        
        },
        (error: any) => {
          console.log(error.error);
        }
      )
  }

  startCountDown() {
    this.timerStarted = true;
  }

  countDownDone(message: string) {
    this.timerStarted = false;
    this.timerDone = true;
    this.dialog.open(this.notification);
  }

  checkCorrectAnswer() {
    if(this.task.isMultiSelect) {

    } 
    else {
      if(this.task.answers.some((a) => a.offeredAnswer === this.checkedAnswer && a.isCorrect)) {
        this.theoryResult = true;
        this.points = this.task.points;
      }
    }
    this.submitTask().then(() => {
      this.done = true;
    })
  }

  submitTask() {
    var promise = new Promise((resolve, reject) => {
      var playerTask = {
        playerId : this.authService.getAuthorized().userId,
        assignmentId : this.task.id,
        scoredPoints: this.points,
        percentage: this.points / this.task.points,
        answers : this.task.isCoding ? '' : (this.task.isMultiSelect ? this.checkedAnswers.join(',') : this.checkedAnswer),
        playersCode : this.task.isCoding ? this.value : ''
      }
      this.gameService.insertPlayerTask(playerTask)
      .subscribe(
        (Response) => {  
            if(Response.status == 200) {
              resolve('done');
            }         
        },
        (error: any) => {
            console.log(error);  
            this.loading = false;
            reject();         
        }); 
      })
      return promise;
  }

  compile() {
    this.loading = true;
    var code = {code : this.value, inputs: this.args}
    this.gameService.runCode(code)
    .subscribe(
      (Response) => {  
          if(Response.status == 200) {
            if(Response.body.error != '') {
              this.error = true;
              this.compileResult = Response.body.error;
              this.loading = false; 
            } 
            else {
              this.error = false;
              this.compileResult = Response.body.result;
              this.loading = false; 
            }
          }         
      },
      (error: any) => {
          console.log(error);  
          this.loading = false;         
      }); 
     
  }



  onBack(): void {
    this.router.navigate(["/map-info/", this.mapId]);
  }
  goBack(): void {
    this.onBack();
  }

  ngOnDestroy() {
    if (this.sub) this.sub.unsubscribe();
  }

}

