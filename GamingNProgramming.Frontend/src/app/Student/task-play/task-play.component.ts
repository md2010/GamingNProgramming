import { Component, TemplateRef, ViewChild, ElementRef, Renderer2  } from '@angular/core';
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
  submitCodeResult : any | null = null;
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
  executionTime : number = 0
  alreadyPlayed : boolean = false;

  task! : Assignment;
  taskId! : string | null
  isDefaultMap! : boolean;
  mapId! : string;
  levelId : string | null = null;

  value: string = '';
  editorOptions = { theme: 'vs-dark', language: 'c' };
  model: NuMonacoEditorModel = {
    language: "c"
  }; 

  @ViewChild('notification', { static: true }) notification!: TemplateRef<any>;
  @ViewChild('badge', { static: true }) badge!: TemplateRef<any>;
  @ViewChild('div') div : any;

  timeUpMessage = 'Vrijeme je isteklo.'
  missingInputMessage = 'Za ovaj zadatak su potrebni ulazni parametri koje je potrebno unijeti u polje \'Unesi ulazne podatke\''

  constructor( private route: ActivatedRoute, private router: Router, private gameService: GameService, private authService : AuthService, public dialog: MatDialog,  private renderer2: Renderer2,
    private elementRef: ElementRef) { 
    this.isDefaultMap = this.router.getCurrentNavigation()!.extras!.state!['isDefaultMap'];
    this.mapId = this.router.getCurrentNavigation()!.extras!.state!['mapId'];
    this.levelId = this.router.getCurrentNavigation()!.extras!.state!['levelId'];
   }

  ngOnInit() {
    this.sub = this.route.paramMap.subscribe((params) => {
      this.taskId = params.get('id');
    });
    this.checkIfPlayed().then(() => {
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
    })
  }

  checkIfPlayed() {
    var promise = new Promise((resolve,reject) => {
      this.gameService.getPlayerTask(this.authService.getAuthorized().userId!, this.mapId, this.taskId!)
      .subscribe(
        (Response) => {
          if(Response.body && Response.body.length > 0) {            
            //this.alreadyPlayed = true;
            //reject();
          }           
          resolve('done');                                                
        },
        (error: any) => {
          reject();
          console.log(error.error);
        }
      );     
    }) 
    return promise;
  }

  startCountDown() {
    this.timerStarted = true;
  }

  countDownDone(message: string) {
    this.timerStarted = false;
    this.timerDone = true;
    this.dialog.open(this.notification, { data: "Vrijeme je isteklo." });
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

  submit() {
    this.loading = true;
    if(this.task.isCoding) {
      this.submitCode().then(() => {
        this.submitTask().then(() => {
          this.done = true;
          this.loading = false;
          if (this.task.hasBadge && this.points === this.task.points) {
            this.surprise();
          }
        })
      })
    } 
  }

  submitTask() {
    var promise = new Promise((resolve, reject) => {
      var playerTask = {
        playerId : this.authService.getAuthorized().userId,
        assignmentId : this.task.id,
        levelId : this.levelId,
        scoredPoints: this.points,
        percentage: (this.points / this.task.points)*100,
        answers : this.task.isCoding ? '' : (this.task.isMultiSelect ? this.checkedAnswers.join(',') : this.checkedAnswer),
        playersCode : this.task.isCoding ? this.value : '',
        mapId : this.mapId,
        executionTime: this.executionTime,
        badgeId : this.task.hasBadge ? this.task.badgeId : ''
      }
      this.gameService.insertPlayerTask(playerTask)
      .subscribe(
        (Response) => {  
            if(Response.status == 200) {
              this.loading = false;
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

  submitCode() {   
    var promise = new Promise((resolve, reject) => {
    var code = {code : this.value, inputs: this.args ?? "", testCases: this.task.testCases, points: this.task.points}
    this.gameService.submitCode(code)
    .subscribe(
      (Response) => {  
          if(Response.status == 200) {
            if(Response.body.error) {
              this.error = true;
              this.compileResult = Response.body.error;
              reject();
            }
            if(Response.body.results) {
              this.error = false;
              this.points = Response.body.points;
              this.submitCodeResult = Response.body.results;
              this.executionTime = Response.body.executionTime;
              resolve('done');
            }             
          }         
      },
      (error: any) => {
          console.log(error); 
          reject();          
      });
    });
    return promise; 
  } 


  compile() {
    if (this.task.hasArgs && this.args === null) {
      this.dialog.open(this.notification, { data: "Za ovaj zadatak su potrebni ulazni parametri koje je potrebno unijeti u polje 'Unesi ulazne podatke'." });
    }
    else {
      this.loading = true;
      var code = {code : this.value, inputs: this.args ?? ""}
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

  }

  surprise(): void {
    this.dialog.open(this.badge, { data: this.task.badge!.path, backdropClass: 'bdp'});
    const confetti = require('canvas-confetti');
    const canvas = this.renderer2.createElement('canvas');
 
    this.renderer2.appendChild(this.elementRef.nativeElement, canvas);
 
    const myConfetti = confetti.create(canvas, {
      resize: true,
      particleCount: 300,
      spread: 160,
      origin: {y: 0.6}
    });
 
    myConfetti();

    setTimeout(() => {
      myConfetti.reset();
      this.renderer2.removeChild(this.elementRef.nativeElement, canvas);
    }, 5000);
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

