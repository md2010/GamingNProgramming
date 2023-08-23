import { Component, ViewChild, AfterViewInit, ElementRef, Input, SimpleChanges  } from '@angular/core';
import { Level } from 'src/app/classes/Classes';
import { AuthService } from 'src/app/services/AuthService';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-draw-map',
  templateUrl: './draw-map.component.html',
  styleUrls: ['./draw-map.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class DrawMapComponent {

  @Input() imgSrc : string = '';
  @Input() levels! : Array<Level>; //10 max
  @Input() draw : boolean = false;
  @Input() writeText : boolean = false;

  @Input() userLevel : number | null = null
  @Input() userTask : number | null = null
  @Input() avatarSrc : string = ''
  @Input() mapTitle : string = ''

  @ViewChild('canvas') canvas!: ElementRef;

  ctx!: CanvasRenderingContext2D;
  image = new Image(1000, 800);
  avatar = new Image(40,40);
  circles! : any;
  canvasWidth : number = 1000
  canvasHeight : number = 800

  constructor(private authService: AuthService) {}

  ngOnChanges(changes: SimpleChanges) {
    if(changes['imgSrc'])
    {
      this.imgSrc = changes['imgSrc'].currentValue
    }
    else if(changes['levels']) {
      this.levels = changes['levels'].currentValue;
    }
    this.ngOnInit();
  }

  ngOnInit() {
    const img = this.image;
    if(this.writeText) {
      this.canvasWidth = 400;
      this.canvasHeight = 350;
      this.image.onload = () => {
        this.image.width = 400;
        this.image.height = 350;
        this.ctx.drawImage(img, 0, 0, img.width, img.height); 
        this.ctx.textAlign = "center";
        this.ctx.font = "bold 22px Calibri";
        this.ctx.fillStyle = "black";
        this.ctx.fillText(this.mapTitle, img.width/2, img.height/2); 
      }      
    }
    else {
      this.image.onload = () => {
        this.ctx.drawImage(img, 0, 0, img.width, img.height);         
        if(this.draw) {
          this.drawCircles();
        }   
      };
    }
    
    this.image.src = this.imgSrc;
  }

  ngAfterViewInit() {
    this.ctx = this.canvas.nativeElement.getContext('2d'); 
    if(this.draw) {
      this.drawCircles();
    }   
  }

  drawCircles() {
    for (let i = 0; i <= this.levels.length - 1; i++) {
      let level = this.levels[i];
      for (let j = 0; j <= level.assignments.length - 1; j++) {
        this.ctx.beginPath();
        let x = 25 + j * 75; // x coordinate
        let y = 25 + i * 75; // y coordinate
        let radius = 20; 
        let startAngle = 0; 
        let endAngle = 2 * Math.PI; 
        this.ctx.arc(x, y, radius, startAngle, endAngle); 
        if(this.authService.getAuthorized().roleName === 'Student' && this.userLevel != null && this.userTask != null && i == this.userLevel && j == this.userTask) {
          var img = this.avatar;
          this.avatar.onload = () => {
          this.ctx.drawImage(img, x-15, y-15, img.width, img.height);
        }
        this.avatar.src = this.avatarSrc     
        }    
        else {
          this.ctx.fillStyle = "yellow";
          this.ctx.fill();
        }
      }
    }
  }
}
