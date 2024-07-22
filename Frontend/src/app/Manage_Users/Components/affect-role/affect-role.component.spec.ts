import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AffectRoleComponent } from './affect-role.component';

describe('AffectRoleComponent', () => {
  let component: AffectRoleComponent;
  let fixture: ComponentFixture<AffectRoleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AffectRoleComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AffectRoleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
