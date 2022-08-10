import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PeripheralsComponent } from './peripherals.component';

describe('PeripheralsComponent', () => {
  let component: PeripheralsComponent;
  let fixture: ComponentFixture<PeripheralsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PeripheralsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PeripheralsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
