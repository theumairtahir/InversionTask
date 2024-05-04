import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RootAscendantComponent } from './root-ascendant.component';

describe('RootAscendantComponent', () => {
  let component: RootAscendantComponent;
  let fixture: ComponentFixture<RootAscendantComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RootAscendantComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RootAscendantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
