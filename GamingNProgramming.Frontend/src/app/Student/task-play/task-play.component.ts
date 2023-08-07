import { Component, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router'
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NuMonacoEditorModule, NuMonacoEditorModel } from '@ng-util/monaco-editor';
import { GameService } from 'src/app/services/GameService';
import { SpinnerComponentComponent } from 'src/app/spinner-component/spinner-component.component';
import { Assignment, PlayerTask } from 'src/app/classes/Classes';
import {MatCheckboxChange, MatCheckboxModule} from '@angular/material/checkbox';
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
  offeredAnswers : Array<OfferedAnswer> = [];
  checkedAnswer : string | null = null;
  theoryResult :  boolean = false
  correctAnswer : string | undefined = '';
  correctAnswers : string = ''
  done :  boolean = false
  points : number = 0

  task! : Assignment;
  taskId! : string | null
  isDefaultMap! : boolean;
  mapId! : string;
  playersTask : PlayerTask | null = null;

  value: string = '';
  editorOptions = { theme: 'vs-dark', language: 'c' };
  model: NuMonacoEditorModel = {
    language: "c"
  }; 

  @ViewChild('notification', { static: true }) notification!: TemplateRef<any>;

  constructor( private route: ActivatedRoute, private router: Router, private gameService: GameService, private authService : AuthService, public dialog: MatDialog,) { 
    this.isDefaultMap = this.router.getCurrentNavigation()!.extras!.state!['isDefaultMap'];
    this.mapId = this.router.getCurrentNavigation()!.extras!.state!['mapId'];
    this.playersTask = this.router.getCurrentNavigation()!.extras!.state!['playersTask'];
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
            if (!this.task.isCoding) {
              this.correctAnswer = this.task.answers.find((a) => a.isCorrect === true)?.offeredAnswer;
              if(this.task.isMultiSelect) {
                this.task.answers.forEach(a => {
                  this.offeredAnswers.push({value: a.offeredAnswer, checked: false});
                  if(a.isCorrect)
                    this.correctAnswers += ' ' + a.offeredAnswer
                });
              }
            }
            else {
              this.value = this.task.initialCode;    
            }    
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

  checkBoxChanged(obj : OfferedAnswer) {
    if(obj.checked) {
      this.checkedAnswers.push(obj.value)
    }
    else {
      this.checkedAnswers = this.checkedAnswers.filter(a => a !== obj.value);
    }
  }

  checkCorrectAnswer() {
    if(this.task.isMultiSelect) {
      var correct = 0;
      this.task.answers.forEach(answer => {
        if(this.checkedAnswers.some((a) => a === answer.offeredAnswer && answer.isCorrect))
        {
          correct++;
          this.theoryResult = true;
        }
      })
      this.points = Math.round(correct/this.task.points)
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
        percentage: (this.points / this.task.points)*100,
        answers : this.task.isCoding ? '' : (this.task.isMultiSelect ? this.checkedAnswers.join(',') : this.checkedAnswer),
        playersCode : this.task.isCoding ? this.value : '',
        mapId : this.mapId
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
    var code = {code : this.value, inputs: this.args ?? "", testCases: this.task.testCases}
    this.gameService.submitCode(code)
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

interface OfferedAnswer {
  value: string 
  checked : boolean 
}

