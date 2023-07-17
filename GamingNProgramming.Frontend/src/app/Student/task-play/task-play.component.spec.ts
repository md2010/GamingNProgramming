import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskPlayComponent } from './task-play.component';

describe('TaskPlayComponent', () => {
  let component: TaskPlayComponent;
  let fixture: ComponentFixture<TaskPlayComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TaskPlayComponent]
    });
    fixture = TestBed.createComponent(TaskPlayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
