import { Component, Input, Output, EventEmitter } from '@angular/core';
import { map, timer, takeWhile, finalize } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'timer',
  templateUrl: './timer.component.html',
  styleUrls: ['./timer.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule]
})
export class TimerComponent {

  @Input() seconds = 10;
  @Output() countDownEvent = new EventEmitter<string>();

   timeRemaining = timer(0, 1000)
  .pipe(
    map(n => (this.seconds - n) * 1000),
    takeWhile(n => n >= 0),
    finalize(() => 
      this.countDownEvent.emit('done')
    )
  )
  
}