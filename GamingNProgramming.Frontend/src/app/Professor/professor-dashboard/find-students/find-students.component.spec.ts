import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FindStudentsComponent } from './find-students.component';

describe('FindStudentsComponent', () => {
  let component: FindStudentsComponent;
  let fixture: ComponentFixture<FindStudentsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FindStudentsComponent]
    });
    fixture = TestBed.createComponent(FindStudentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
