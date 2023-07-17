import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyMapsComponent } from './my-maps.component';

describe('MyMapsComponent', () => {
  let component: MyMapsComponent;
  let fixture: ComponentFixture<MyMapsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MyMapsComponent]
    });
    fixture = TestBed.createComponent(MyMapsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
