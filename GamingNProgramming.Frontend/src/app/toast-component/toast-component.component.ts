import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ToastModule } from '@coreui/angular';

@Component({
  selector: 'app-toast-component',
  templateUrl: './toast-component.component.html',
  styleUrls: ['./toast-component.component.css'],
  standalone: true,
  imports: [ToastModule]
})
export class ToastComponentComponent {
  
  @Input() message = '';
  @Output() timeElapsedEvent = new EventEmitter<string>();

  ngOnInit() {
    this.visible = true;
  }

  constructor() {}

  position = 'top-end';
  visible = false;
  percentage = 0;

  onVisibleChange($event: boolean) {
    this.visible = $event;
    this.percentage = !this.visible ? 0 : this.percentage;
    if(this.percentage == 0 && !this.visible) {
      this.timeElapsedEvent.emit();
    }
  }

  onTimerChange($event: number) {
    this.percentage = $event * 25;
  }
}
