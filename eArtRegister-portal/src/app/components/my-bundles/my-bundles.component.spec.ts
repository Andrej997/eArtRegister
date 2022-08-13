import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyBundlesComponent } from './my-bundles.component';

describe('MyBundlesComponent', () => {
  let component: MyBundlesComponent;
  let fixture: ComponentFixture<MyBundlesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyBundlesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyBundlesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
