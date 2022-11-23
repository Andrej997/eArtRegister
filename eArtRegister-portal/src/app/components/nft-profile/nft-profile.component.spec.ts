import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NftProfileComponent } from './nft-profile.component';

describe('NftProfileComponent', () => {
  let component: NftProfileComponent;
  let fixture: ComponentFixture<NftProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NftProfileComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NftProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
