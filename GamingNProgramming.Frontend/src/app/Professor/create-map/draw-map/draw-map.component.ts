import { Component, ViewChild, AfterViewInit, ElementRef, Input  } from '@angular/core';

@Component({
  selector: 'app-draw-map',
  templateUrl: './draw-map.component.html',
  styleUrls: ['./draw-map.component.css'],
  standalone: true
})
export class DrawMapComponent {

  @Input() imgSrc : string = '';
  @ViewChild('canvas') canvas!: ElementRef;
  ctx!: CanvasRenderingContext2D;
  image = new Image(1000, 800);
  avatar = new Image(40,40);
  circles! : any;

  constructor() {}

  ngOnInit() {
    const img = this.image;
    this.image.onload = () => {
      this.ctx.drawImage(img, 0, 0, img.width, img.height);
    };
    this.image.src = this.imgSrc;
  }

  ngAfterViewInit() {
    this.ctx = this.canvas.nativeElement.getContext('2d');
    this.drawCircles();
  }

  drawCircles() {
    for (let i = 0; i <= 10; i++) {
      for (let j = 0; j <= 12; j++) {
        this.ctx.beginPath();
        let x = 25 + j * 75; // x coordinate
        let y = 25 + i * 75; // y coordinate
        let radius = 20; 
        let startAngle = 0; 
        let endAngle = 2 * Math.PI; 
        this.ctx.arc(x, y, radius, startAngle, endAngle);      
        if(i == 3 && j == 4)
        {
          const img = this.avatar;
          this.avatar.onload = () => {
            this.ctx.drawImage(img, x-15, y-15, img.width, img.height);
          };
          this.avatar.src = "../assets/images/man-avatar-1.png"
          //this.ctx.fillStyle = "green";
        }
        else {
          this.ctx.fillStyle = "yellow";
          this.ctx.fill();
        }
      }
    }
  }
}
