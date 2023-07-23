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
import { AuthService } from 'src/app/services/AuthService';

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
  numOfAssignments = 0;
  showDrawMap = false;
  activeDefaultMap = false;

  @ViewChild('notification', { static: true }) notification!: TemplateRef<any>;

  constructor(private router: Router, public dialog: MatDialog, private gameService : GameService, private authService: AuthService) {}

   ngOnInit() {
    this.gameService.getMapForEditing(this.authService.getAuthorized().userId!) 
    .subscribe(
      (Response) => {
        if(Response) {
          this.setData(Response.body);          
        }
        else {
          this.levels = new Array<Level>();
          var tasks : Array<Assignment> = []
          tasks.push(new Assignment())
          this.levels.push(new Level(tasks))
        }
      },
      (error: any) => {
        console.log(error.error);
      }
    )} 
    
    setData(result: any) {
      if(result.path.length > 2)
      {
        this.onMapSelected(result.path);
      }
      this.title = result.title;
      this.description = result.description;
      this.levels = result.levels;
    }

  onMapSelected(value: string) {
    this.showDrawMap = false;
    this.mapPath = value;
    this.showDrawMap = true;
    if(value.includes('default')) {
      this.activeDefaultMap = true;
    }
    else {
      this.activeDefaultMap = false;
    }
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
    this.numOfAssignments ++;
    this.levels[levelNumber].assignments.push(new Assignment())
  }
  deleteTask(i : number, levelNumber: number) {
    this.levels[levelNumber].assignments.splice(i, 1);
  }

  openDialog(i : number, levelNumber: number) {
    let dialogRef = this.dialog.open(CreateTaskDialogComponent, {
      width: '1500px',
      height: '900px', 
      data: { task: this.levels[levelNumber].assignments[i], index : i, disableEdit : false },    
    });
    dialogRef.afterClosed().subscribe(result => {
      this.levels[levelNumber].assignments[i] = result.task;
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



