import { Component, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { map, timer, takeWhile, finalize, Observable } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { subscribe } from 'diagnostics_channel';

@Component({
  selector: 'timer',
  templateUrl: './timer.component.html',
  styleUrls: ['./timer.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule]
})
export class TimerComponent {

  @Input() seconds = 10;
  @Output() countDownEvent = new EventEmitter<Date>();

  @ViewChild('timer') timer!: ElementRef;

  start : Date = new Date()

  timeRemaining = timer(0, 1000)
  .pipe(
    map(n => (this.seconds - n) * 1000),
    takeWhile(n => n >= 0),
    finalize(() => {
      this.countDownEvent.emit(this.start)
    })
  )
  
}