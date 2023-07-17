import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FindFriendsComponentComponent } from './find-friends-component.component';

describe('FindFriendsComponentComponent', () => {
  let component: FindFriendsComponentComponent;
  let fixture: ComponentFixture<FindFriendsComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FindFriendsComponentComponent]
    });
    fixture = TestBed.createComponent(FindFriendsComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
