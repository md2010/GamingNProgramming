import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectAvatarComponent } from './select-avatar.component';

describe('SelectAvatarComponent', () => {
  let component: SelectAvatarComponent;
  let fixture: ComponentFixture<SelectAvatarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SelectAvatarComponent]
    });
    fixture = TestBed.createComponent(SelectAvatarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
