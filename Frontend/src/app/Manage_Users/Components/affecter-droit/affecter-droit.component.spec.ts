import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AffecterDroitComponent } from './affecter-droit.component';

describe('AffecterDroitComponent', () => {
  let component: AffecterDroitComponent;
  let fixture: ComponentFixture<AffecterDroitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AffecterDroitComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AffecterDroitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
