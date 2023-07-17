import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router'
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NuMonacoEditorModule, NuMonacoEditorEvent, NuMonacoEditorModel } from '@ng-util/monaco-editor';
import { GameService } from 'src/app/services/GameService';
import { SpinnerComponentComponent } from 'src/app/spinner-component/spinner-component.component';

@Component({
  selector: 'app-task-play',
  templateUrl: './task-play.component.html',
  styleUrls: ['./task-play.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NuMonacoEditorModule, SpinnerComponentComponent]
})

export class TaskPlayComponent {

  id : string | null = null
  sub : any;
  compileResult : string | null = null
  error : boolean = true
  args : string | null = null
  loading = false;

  constructor( private route: ActivatedRoute, private router: Router, private gameService: GameService) {  }

  ngOnInit() {
    this.sub = this.route.paramMap.subscribe((params) => {
      console.log(params);
      this.id = params.get('id');
    });
  }  

  value: string = 'const a = 1;';
  editorOptions = { theme: 'vs-dark', language: 'c' };
  model: NuMonacoEditorModel = {
    language: "c"
  }; 

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



  onBack(id : string): void {
    this.router.navigate(['map-info', 1]);
  }

  ngOnDestroy() {
    if (this.sub) this.sub.unsubscribe();
  }

}

