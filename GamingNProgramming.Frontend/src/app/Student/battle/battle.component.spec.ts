import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BattleComponent } from './battle.component';

describe('BattleComponent', () => {
  let component: BattleComponent;
  let fixture: ComponentFixture<BattleComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BattleComponent]
    });
    fixture = TestBed.createComponent(BattleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
