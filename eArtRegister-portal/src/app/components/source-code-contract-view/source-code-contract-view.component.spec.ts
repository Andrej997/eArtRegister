import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SourceCodeContractViewComponent } from './source-code-contract-view.component';

describe('SourceCodeContractViewComponent', () => {
  let component: SourceCodeContractViewComponent;
  let fixture: ComponentFixture<SourceCodeContractViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SourceCodeContractViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SourceCodeContractViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
