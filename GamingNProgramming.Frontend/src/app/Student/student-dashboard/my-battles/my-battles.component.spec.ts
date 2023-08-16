import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyBattlesComponent } from './my-battles.component';

describe('MyBattlesComponent', () => {
  let component: MyBattlesComponent;
  let fixture: ComponentFixture<MyBattlesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MyBattlesComponent]
    });
    fixture = TestBed.createComponent(MyBattlesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
