import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TraiterIncidentComponent } from './traiter-incident.component';

describe('TraiterIncidentComponent', () => {
  let component: TraiterIncidentComponent;
  let fixture: ComponentFixture<TraiterIncidentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TraiterIncidentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TraiterIncidentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
