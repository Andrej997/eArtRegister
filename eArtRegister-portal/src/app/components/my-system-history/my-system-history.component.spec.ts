import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MySystemHistoryComponent } from './my-system-history.component';

describe('MySystemHistoryComponent', () => {
  let component: MySystemHistoryComponent;
  let fixture: ComponentFixture<MySystemHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MySystemHistoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MySystemHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
