import { Component, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AvatarModule } from '@coreui/angular';
import { MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

import { DrawMapComponent } from './draw-map/draw-map.component';
import { CreateTaskDialogComponent } from './create-task-dialog/create-task-dialog.component';
import { Assignment, Level, Map } from 'src/app/classes/Classes';
import { GameService } from 'src/app/services/GameService';

@Component({
  selector: 'app-create-map',
  templateUrl: './create-map.component.html',
  styleUrls: ['./create-map.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, DrawMapComponent, AvatarModule, CreateTaskDialogComponent, MatDialogModule]
})
export class CreateMapComponent {

  levels : Level[] = []
  mapPath : string = ''
  title : string = ''
  description : string = ''

  @ViewChild('notification', { static: true }) notification!: TemplateRef<any>;

  constructor(private router: Router, public dialog: MatDialog, private gameService : GameService) {
    this.levels = new Array<Level>();
    var tasks = new Array<Assignment>();
    tasks.push(new Assignment())
    this.levels.push(new Level(tasks))
   }

  onMapSelected(value: string) {
    this.mapPath = value;
  }

  addLevel() {
    var tasks = new Array<Assignment>();
    tasks.push(new Assignment())
    this.levels.push(new Level(tasks))
    console.log(this.levels)
  }
  deleteLevel(i : number) {
    this.levels.splice(i, 1);
    console.log(this.levels)
  }

  addTask(levelNumber: number) {
    this.levels[levelNumber].tasks.push(new Assignment())
  }
  deleteTask(i : number, levelNumber: number) {
    this.levels[levelNumber].tasks.splice(i, 1);
  }

  openDialog(i : number, levelNumber: number) {
    let dialogRef = this.dialog.open(CreateTaskDialogComponent, {
      width: '1500px',
      height: '900px', 
      data: { task: this.levels[levelNumber].tasks[i], index : i },    
    });
    dialogRef.afterClosed().subscribe(result => {
      this.levels[levelNumber].tasks[i] = result.task;
      });
  }

  save() {
    var map = new Map(this.title, this.description, this.mapPath, true, this.levels);
    this.gameService.saveMap(map)
    .subscribe(
      (Response) => {
        if(Response.body) {
          
        }
      },
      (error: any) => {
        console.log(error.error);

      }
    )
  }

  saveAndContinueEditing() {
    var map = new Map(this.title, this.description, this.mapPath, true, this.levels);
    this.dialog.open(this.notification);
  }
}



