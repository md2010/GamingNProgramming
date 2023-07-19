import { Component, Inject, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef} from '@angular/material/dialog';
import { NuMonacoEditorModule, NuMonacoEditorEvent, NuMonacoEditorModel } from '@ng-util/monaco-editor';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { Answer, Assignment,TestCase, Badge } from 'src/app/classes/Classes';
import {MatRadioModule} from '@angular/material/radio';
import { AvatarModule } from '@coreui/angular';

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
    public dialog: MatDialog
    ){ 
      this.task = data.task
      this.task.testCases.push(new TestCase());
      this.task.answers.push(new Answer());
      this.task.answers.push(new Answer());

      this.task.initialCode = 
    '/*\nOvdje unesi kod koji će se prikazati kao predložak. \n Važno je napomenuti da ' +
    'se zadaci pokreću preko komandne \n linije te ukoliko su ulazni argumenti brojevi,' + 
    '\n a ne tekstulani (string), potrebno je ' +
    'ulaze pretvoriti \n u brojeve. Primjer: a = argv[1]; val = atoi(a);\n*/';
  }

  @ViewChild('selectBadge', { static: true }) selectBadgeDialog!: TemplateRef<any>;

  correctAnswer! : string
  task!: Assignment

  editorOptions = { theme: 'vs-dark', language: 'c' };
  model: NuMonacoEditorModel = {
    language: "c"
  }; 

  badges : Array<Badge> = [
    {path: '../assets/images/badge-1.png', id: '1'}, 
    {path: '../assets/images/badge-2.png', id: '2'},
    {path: '../assets/images/badge-3.png', id: '3'}
  ]

  theory() {
    this.task.isCoding = false;
  }

  coding() {
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
  onBadgeSelected(id : string) {
    this.task.badgeId = id;
  }

  save() {
    this.task.answers.forEach(answer => {
      if(answer.offeredAnswer === this.correctAnswer) {
        answer.isCorrect = true;
      }
    }); 
    this.dialogRef.close({task: this.task});
  }
}

interface Data {
  task: Assignment
}


