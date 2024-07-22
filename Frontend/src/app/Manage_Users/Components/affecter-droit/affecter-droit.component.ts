import { Component, OnInit } from '@angular/core';
import { RowComponent, ColComponent, TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, FormSelectDirective } from '@coreui/angular';
import { ReactiveFormsModule,FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common'; // Importer CommonModule

import { RoleService } from '../../_services/role.service';

@Component({
  selector: 'app-affecter-droit',
  standalone: true,
  imports: [RowComponent, ColComponent, TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, FormSelectDirective, ReactiveFormsModule,CommonModule],
  templateUrl: './affecter-droit.component.html',
  styleUrl: './affecter-droit.component.scss'
})
export class AffecterDroitComponent implements OnInit {

  roles: any[] = [];
  droits: any[] = [];
  selectedRoleId: number | null = null;
  assignForm: FormGroup;

  constructor(private roleService: RoleService, private fb: FormBuilder) {
    this.assignForm = this.fb.group({
      roleId: [null, Validators.required],
      droitIds: [[], Validators.required]
    });
  }

  ngOnInit(): void {
    this.getRoles();
  }

  getRoles(): void {
    this.roleService.listRoles().subscribe((data) => {
      this.roles = data;
    });
  }

  onRoleChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.selectedRoleId = Number(target.value);
    this.getDroits();
  }

  getDroits(): void {
    this.roleService.getDroits().subscribe((data) => {
      this.droits = data;
    });
  }

  onSubmit(): void {
    if (this.assignForm.valid) {
      this.roleService.assignDroitsToRole(this.assignForm.value.roleId, this.assignForm.value.droitIds).subscribe(
        (response) => {
          // Afficher un message de succès
          alert('Droits assigned successfully: ' + response.message); // Utilisez la propriété appropriée de votre réponse
        },
        (error) => {
          // Afficher un message d'erreur
          alert('Error assigning droits: ' + error.message); // Utilisez la propriété appropriée de l'erreur
        }
      );
    }
  }
  

}
