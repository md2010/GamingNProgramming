import { Component, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router'
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NuMonacoEditorModule, NuMonacoEditorModel } from '@ng-util/monaco-editor';
import { GameService } from 'src/app/services/GameService';
import { SpinnerComponentComponent } from 'src/app/spinner-component/spinner-component.component';
import { Assignment, PlayerTask } from 'src/app/classes/Classes';
import { MatCheckboxModule} from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { AuthService } from 'src/app/services/AuthService';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ToastComponentComponent } from 'src/app/toast-component/toast-component.component';

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
    MatDialogModule,
    ToastComponentComponent
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
  showToast : boolean = false
  message : string = ''

  task! : Assignment;
  taskId! : string | null
  mapId! : string;
  playersTask : PlayerTask | null = null;

  playerId: string | null = null
  newPoints : number | null = null

  value: string = '';
  editorOptions = { theme: 'vs-dark', language: 'cpp' };
  model: NuMonacoEditorModel = {
    language: "cpp"
  }; 
  code = ''
  description = ''

  @ViewChild('notification', { static: true }) notification!: TemplateRef<any>;

  constructor( private route: ActivatedRoute, private router: Router, private gameService: GameService, public dialog: MatDialog, private authService : AuthService) { 
    this.mapId = this.router.getCurrentNavigation()!.extras!.state!['mapId'];
    this.playerId = this.router.getCurrentNavigation()!.extras!.state!['playerId'];
    if(!this.playerId) {
      this.playerId = this.authService.getAuthorized().userId;
    }
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
                if(this.task.description.includes('<code>')) {
                  this.description = this.task.description.substring(0 , this.task.description.indexOf('<code>'));
                  this.code = this.task.description.substring(this.task.description.indexOf('<code>')+6, this.task.description.indexOf('</code>'))
                }
                else {
                  this.description = this.task.description;
                }
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
      this.gameService.getPlayerTask(this.playerId!, this.mapId)
      .subscribe(
        (Response) => {
          if(Response.body && Response.body.length > 0) {
            var playerTasks = Response.body;
            this.playersTask = playerTasks.find((a : any) => a.assignmentId === this.taskId);
            this.newPoints = this.playersTask!.scoredPoints
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

  updatePoints() {
    this.loading = true;
    if(!this.newPoints) {
      this.dialog.open(this.notification, { data: 'Unesi vrijednost!'});
    }
    if(this.newPoints! > this.task.points || this.newPoints! < 0 || this.newPoints === this.playersTask?.scoredPoints) {
      this.dialog.open(this.notification, { data: 'Neispravan unos bodova!'});
    }
    else {
      var data = {playerTaskId: this.playersTask!.id, newPoints: this.newPoints }
      this.gameService.updateScoredPoints(data)
      .subscribe(
        (Response) => {  
            if(Response.status == 200) {
              this.loading = false;
              this.message = "Bodovi su ažurirani."
              this.showToast = true;
            }         
        },
        (error: any) => {
            console.log(error); 
            this.loading = false;                
        });
    }
  }

  onToastMessageElapsed() {
    this.showToast = false;
}
  
  onBack(): void {
    if(this.role === 'Student')
      this.router.navigate(["/map-info/", this.mapId]);
    else 
      this.router.navigate(["/review/", this.playerId]);
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

