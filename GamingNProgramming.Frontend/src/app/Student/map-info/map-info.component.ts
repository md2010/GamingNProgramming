import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Map } from 'src/app/classes/Classes';
import { AuthService } from 'src/app/services/AuthService';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { CreateTaskDialogComponent } from 'src/app/Professor/create-map/create-task-dialog/create-task-dialog.component';
import { DrawMapComponent } from 'src/app/Professor/create-map/draw-map/draw-map.component';

@Component({
  selector: 'app-map-info',
  templateUrl: './map-info.component.html',
  styleUrls: ['./map-info.component.css'],
  standalone: true,
  imports: [CommonModule, CreateTaskDialogComponent, DrawMapComponent]
  
})
export class MapInfoComponent {

  id : string | null = null
  sub : any;
  map! : Map;
  role! : string | null

  constructor( private route: ActivatedRoute, private router: Router, private authService : AuthService, public dialog: MatDialog) { 
    this.map = this.router.getCurrentNavigation()!.extras!.state!['map'];
   }

  ngOnInit() {
    this.sub = this.route.paramMap.subscribe((params) => {
      console.log(params);
      this.id = params.get('id');
    });
    this.role = this.authService.getAuthorized().roleName
  }

  openDialog(i : number, levelNumber: number) {
    let dialogRef = this.dialog.open(CreateTaskDialogComponent, {
      width: '1500px',
      height: '900px', 
      data: { task: this.map.levels[levelNumber].assignments[i], index : i, disableEdit : true },    
    });
  }

  onBack(): void {
    if(this.role == 'Professor') {
      this.router.navigate(['professor-dashboard']);
    }
    else {
      this.router.navigate(['student-dashboard']);
    }
  }

  ngOnDestroy() {
    if (this.sub) this.sub.unsubscribe();
  }
}
