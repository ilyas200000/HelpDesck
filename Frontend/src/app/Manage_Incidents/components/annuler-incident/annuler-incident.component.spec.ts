import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnnulerIncidentComponent } from './annuler-incident.component';

describe('AnnulerIncidentComponent', () => {
  let component: AnnulerIncidentComponent;
  let fixture: ComponentFixture<AnnulerIncidentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnnulerIncidentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AnnulerIncidentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
