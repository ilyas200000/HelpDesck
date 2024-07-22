import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsulterIncidentComponent } from './consulter-incident.component';

describe('ConsulterIncidentComponent', () => {
  let component: ConsulterIncidentComponent;
  let fixture: ComponentFixture<ConsulterIncidentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConsulterIncidentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ConsulterIncidentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
