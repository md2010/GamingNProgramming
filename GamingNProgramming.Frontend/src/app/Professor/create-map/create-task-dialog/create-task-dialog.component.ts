import { Component, Inject, TemplateRef, ViewChild, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef} from '@angular/material/dialog';
import { NuMonacoEditorModule, NuMonacoEditorEvent, NuMonacoEditorModel } from '@ng-util/monaco-editor';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { Answer, Assignment,TestCase, Badge } from 'src/app/classes/Classes';
import {MatRadioModule} from '@angular/material/radio';
import { AvatarModule } from '@coreui/angular';
import { LookupService } from 'src/app/services/LookupService';

@Component({
  selector: 'app-create-task-dialog',
  templateUrl: './create-task-dialog.component.html',
  styleUrls: ['./create-task-dialog.component.css'],
  standalone: true,
  imports: [
    MatDialogModule, 
    CommonModule, 
    FormsModule, 
    NuMonacoEditorModule, 
    MatSlideToggleModule, 
    MatCheckboxModule, 
    MatRadioModule,
    AvatarModule
  ]
})
export class CreateTaskDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<CreateTaskDialogComponent>, 
    @Inject(MAT_DIALOG_DATA) public data: Data,
    public dialog: MatDialog,
    private lookupService: LookupService
    ){ 
      this.task = data.task
      this.task.badgeId = this.task.badgeId == null ? '' : this.task.badgeId;
      if(this.task.testCases && this.task.testCases.length === 0)
      {
        this.task.testCases = [];
      }
      if(this.task.answers && this.task.answers.length === 0)
      {
        this.task.answers = [];
      }
      this.disableEdit = data.disableEdit;

      this.task.initialCode = 
    '/*\nOvdje unesi kod koji će se prikazati kao predložak. \n Važno je napomenuti da ' +
    'se zadaci pokreću preko komandne \n linije. Ukoliko zadatak zahtjeva ulaz\n'+
    ' treba koristiti int main(int argc, char *argv[]). Ako su ulazni argumenti brojevi,' + 
    '\n a ne tekstulani (string), potrebno je ' +
    'ulaze pretvoriti \n u brojeve. Primjer:\n a = argv[1]; val = atoi(a);\n' +
    '\nint main(int argc, char *argv[])\n{\n}\n\n*/';
  }

  @ViewChild('selectBadge', { static: true }) selectBadgeDialog!: TemplateRef<any>;

  disableEdit : boolean = false;
  correctAnswer! : string
  correctAnswers : Array<string> = []
  task!: Assignment

  editorOptions = { theme: 'vs-dark', language: 'c' };
  model: NuMonacoEditorModel = {
    language: "c"
  }; 

  badges : Array<Badge> = []

  ngOnInit() {
    this.lookupService.getBadges().subscribe(response => {
      this.badges = response;
  })
  }

  theory() {
    this.task.initialCode = '';
    this.task.isCoding = false;
  }

  coding() {
    this.task.initialCode = 
    '/*\nOvdje unesi kod koji će se prikazati kao predložak. \n Važno je napomenuti da ' +
    'se zadaci pokreću preko komandne \n linije te ukoliko su ulazni argumenti brojevi,' + 
    '\n a ne tekstulani (string), potrebno je ' +
    'ulaze pretvoriti \n u brojeve. Primjer: int value = atoi(argv[1]);\n*/';
    this.task.isCoding = true;
  }

  addTestCase() {
    this.task.testCases.push(new TestCase());
  }

  addAnswer() {
    this.task.answers.push(new Answer());
  }

  openDialog() {
    this.dialog.open(this.selectBadgeDialog);
  }
  onHasBadgeChanged(event: any) {
    if(!event.checked) {
      this.task.badgeId = ''
    }
  }
  onBadgeSelected(id : string) {
    this.task.badgeId = id;
  }

  answerChanged(index: number) {
    this.task.answers[index].isCorrect = !this.task.answers[index].isCorrect
  }

  save() {
    if(!this.task.isMultiSelect && !this.task.isCoding) {
      this.task.answers.forEach(answer => {
        if(answer.offeredAnswer === this.correctAnswer) {
          answer.isCorrect = true;
        }
      }); 
    }    
    this.dialogRef.close({task: this.task});
  }
}

interface Data {
  task: Assignment
  disableEdit: boolean
  close() : any
}


