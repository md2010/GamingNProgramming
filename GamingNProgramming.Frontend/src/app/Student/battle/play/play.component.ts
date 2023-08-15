import { Component, TemplateRef, ViewChild, ElementRef, Renderer2, Input, Output, SimpleChanges  } from '@angular/core';
import { ActivatedRoute } from '@angular/router'
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NuMonacoEditorModule, NuMonacoEditorModel } from '@ng-util/monaco-editor';
import { GameService } from 'src/app/services/GameService';
import { SpinnerComponentComponent } from 'src/app/spinner-component/spinner-component.component';
import { Assignment } from 'src/app/classes/Classes';
import { MatCheckboxModule} from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { AuthService } from 'src/app/services/AuthService';
import { TimerComponent } from '../../task-play/timer/timer.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'app-play',
  templateUrl: './play.component.html',
  styleUrls: ['./play.component.css'],
  standalone: true,
  imports: [FormsModule, 
    CommonModule, 
    NuMonacoEditorModule, 
    SpinnerComponentComponent, 
    MatCheckboxModule, 
    MatRadioModule, 
    TimerComponent, 
    MatDialogModule]
})
export class PlayComponent {

  id : string | null = null
  sub : any;
  compileResult : string | null = null
  submitCodeResult : any | null = null;
  error : boolean = true
  args : string | null = null
  loading = false;
  loaded = false;
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
  @Input() taskId! : string | null
  @Input() isLast! : boolean | false
  @Output() nextTaskEvent = new EventEmitter<number>();

  value: string = '';
  editorOptions = { theme: 'vs-dark', language: 'c' };
  model: NuMonacoEditorModel = {
    language: "c"
  }; 

  @ViewChild('notification', { static: true }) notification!: TemplateRef<any>;

  timeUpMessage = 'Vrijeme je isteklo.'
  missingInputMessage = 'Za ovaj zadatak su potrebni ulazni parametri koje je potrebno unijeti u polje \'Unesi ulazne podatke\''

  constructor( private route: ActivatedRoute, private router: Router, private gameService: GameService, private authService : AuthService, public dialog: MatDialog,  private renderer2: Renderer2,
    private elementRef: ElementRef) { }

  ngOnInit() {
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

  ngOnChanges(changes: SimpleChanges) {
    if(changes['taskId'])
    {
      this.taskId = changes['taskId'].currentValue
      this.ngOnInit();
    }  
    if(changes['isLast'])
    {
      this.isLast = changes['isLast'].currentValue  
    }
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
    this.submit()
  }

  submit() {
    this.loading = true;
    if(this.task.isCoding) {
      this.submitCode().then(() => {      
          this.done = true;
          this.loading = false;          
      }
    )}
    else {
      this.done = true;
    } 
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

  next() {
    this.nextTaskEvent.emit(this.points)
  }

  ngOnDestroy() {
    if (this.sub) this.sub.unsubscribe();
  }

}

interface OfferedAnswer {
  value: string 
  checked : boolean 
}

