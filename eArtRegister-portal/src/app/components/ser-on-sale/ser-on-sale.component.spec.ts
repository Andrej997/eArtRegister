import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SerOnSaleComponent } from './ser-on-sale.component';

describe('SerOnSaleComponent', () => {
  let component: SerOnSaleComponent;
  let fixture: ComponentFixture<SerOnSaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SerOnSaleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SerOnSaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
