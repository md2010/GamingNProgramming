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
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-task-view',
  templateUrl: './task-view.component.html',
  styleUrls: ['./task-view.component.css'],
  standalone: true,
  imports: [
    FormsModule, 
    CommonModule, 
    NuMonacoEditorModule, 
    SpinnerComponentComponent, 
    MatCheckboxModule, 
    MatRadioModule, 
    MatDialogModule
  ]
})

export class TaskViewComponent {

  role : string | null = null
  sub : any;
  loading = false;
  loaded = false;
  checkedAnswers : Array<string> = [];
  offeredAnswers : Array<OfferedAnswer> = [];
  checkedAnswer : string | null = null;
  correctAnswer : string | undefined = '';
  correctAnswers : string = ''
  done :  boolean = false
  points : number = 0
  compileResult : string | null = null
  submitCodeResult : any | null = null;
  error : boolean = true

  task! : Assignment;
  taskId! : string | null
  mapId! : string;
  playersTask : PlayerTask | null = null;

  value: string = '';
  editorOptions = { theme: 'vs-dark', language: 'c' };
  model: NuMonacoEditorModel = {
    language: "c"
  }; 

  constructor( private route: ActivatedRoute, private router: Router, private gameService: GameService, private authService : AuthService) { 
    this.mapId = this.router.getCurrentNavigation()!.extras!.state!['mapId'];
    this.role = this.authService.getAuthorized().roleName;
   }

  ngOnInit() {
    this.sub = this.route.paramMap.subscribe((params) => {
      this.taskId = params.get('id');
    });
    var promise = new Promise((resolve, reject) => {
      this.gameService.getTask(this.taskId!) 
      .subscribe(
        (Response) => {
          if(Response) {
            this.task = Response.body;
            if (this.task.isCoding) {
              this.value = this.task.initialCode;
            }  
            else {
              this.correctAnswer = this.task.answers.find((a) => a.isCorrect === true)?.offeredAnswer;
              if(this.task.isMultiSelect) {
                this.task.answers.forEach(a => {
                  this.offeredAnswers.push({value: a.offeredAnswer, checked: false});
                  if(a.isCorrect)
                    this.correctAnswers += ' ' + a.offeredAnswer
                });
              }
            } resolve('done')         
          }                  
        },
        (error: any) => {
          console.log(error.error);
          reject()
        }
      )
    })
    promise.then(() => {     
      this.gameService.getPlayerTask(this.authService.getAuthorized().userId!, this.mapId)
      .subscribe(
        (Response) => {
          if(Response.body && Response.body.length > 0) {
            var playerTasks = Response.body;
            this.playersTask = playerTasks.find((a : any) => a.assignmentId === this.taskId);
            if(!this.task.isCoding) {
              var userAnswers = this.playersTask?.answers.split(',')
              if(this.task.isMultiSelect) {
                this.offeredAnswers.forEach(el => {
                  if(userAnswers!.some((a) => a === el.value)) {
                    el.checked = true;
                  }
                }) 
              }
              else {
                this.checkedAnswer = userAnswers![0];
              }
            }
            else {
              this.value = this.playersTask?.playersCode ?? '';
            }  
          }
          this.loaded = true; 
          if(this.task.isCoding) {
            this.submitCode();
          }                 
        },
        (error: any) => {
          console.log(error.error);
        }
      ) 
    })    
  }

  submitCode() {   
    var promise = new Promise((resolve, reject) => {
    this.loading = true;
    var code = {code : this.value, testCases: this.task.testCases, points: this.task.points}
    this.gameService.submitCode(code)
    .subscribe(
      (Response) => {  
          if(Response.status == 200) {
            if(Response.body.error) {
              this.error = true;
              this.compileResult = Response.body.error;
              this.loading = false;
              reject();
            }
            if(Response.body.results) {
              this.error = false;
              this.loading = false;
              this.points = Response.body.points;
              this.submitCodeResult = Response.body.results;
              resolve('done');
            }             
          }         
      },
      (error: any) => {
          console.log(error); 
          this.loading = false;
          reject();          
      });
    });
    return promise; 
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

