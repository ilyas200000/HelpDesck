import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RowComponent, ColComponent, TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, FormControlDirective, FormDirective, FormLabelDirective, FormSelectDirective, FormCheckComponent, FormCheckInputDirective, FormCheckLabelDirective, ButtonDirective, ColDirective, InputGroupComponent, InputGroupTextDirective } from '@coreui/angular';
import { IncidentService } from '../../_services/incident.service';
import { CommonModule } from '@angular/common';  // Import CommonModule
import { StorageService } from 'src/app/authentification/_AuthServices/storage.service';

@Component({
  selector: 'app-create-incident',
  standalone: true,
  imports: [
    RowComponent, ColComponent, TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent,
    FormControlDirective, ReactiveFormsModule, FormsModule, FormDirective, FormLabelDirective,
    FormSelectDirective, FormCheckComponent, FormCheckInputDirective, FormCheckLabelDirective,
    ButtonDirective, ColDirective, InputGroupComponent, InputGroupTextDirective,CommonModule
  ],
  templateUrl: './create-incident.component.html',
  styleUrls: ['./create-incident.component.scss']
})
export class CreateIncidentComponent implements OnInit {
  incidentForm: FormGroup = this.fb.group({});
  types: any[] = [];
  statuses: any[] = [];
  priorities: any[] = [];
  categories: any[] = [];

  constructor(
    private fb: FormBuilder,
    private incidentService: IncidentService,
    private storageService: StorageService
  ) { }

  ngOnInit(): void {
    this.initForm();
    this.fetchCategories();
    this.fetchStatuts();
    this.fetchPriorities();
  }

  initForm(): void {
    this.incidentForm = this.fb.group({
      description: ['', Validators.required],
      type: [{ value: '', disabled: true }, Validators.required],
      category: ['', Validators.required],
      status: ['', ],
      agencyCode: ['', Validators.required],
      priority: ['',Validators.required]
    });
  }

  fetchTypesByCategory(categoryId: number): void {
    this.incidentService.getTypesByCategory(categoryId).subscribe(
      types => {
        this.types = types;
        // Reset the type select when types are fetched
      },
      error => console.error('Error fetching types by category:', error)
    );
  }

  fetchCategories(): void {
    this.incidentService.getAllCategories().subscribe(
      categories => {
        this.categories = categories;
        // Reset the category select when categories are fetched
      },
      error => console.error('Error fetching categories:', error)
    );
  }
  
  onCategoryChange(): void {
    const categoryId = this.incidentForm.get('category')?.value;
    if (categoryId) {
      this.fetchTypesByCategory(categoryId);
      this.incidentForm.get('type')?.enable();
    }
  }
  

  fetchStatuts(): void {
    this.incidentService.getAllStatuts().subscribe(
      statuses => this.statuses = statuses,
      error => console.error('Error fetching statuses:', error)
    );
  }

  fetchPriorities(): void {
    this.incidentService.getAllPriorites().subscribe(
      priorities => this.priorities = priorities,
      error => console.error('Error fetching priorities:', error)
    );
  }

  onSubmit(): void {
    if (this.incidentForm.valid) {
      const formData = this.incidentForm.value;
      const request  = {
        INCD_DESC: formData.description,
        INCD_PRIO_ID: formData.priority,
        INCD_TYPE_ID: formData.type,
        INCD_STAT_ID: 1,
        agn_code: formData.agencyCode,
        INCD_UTIL_ID: this.storageService.getUser()?.sub,
        INCD_ENT_SG_ID: this.storageService.getUserEntityId(),
        createdBy:  this.storageService.getUser()?.sub
        };
      console.log(request)
      this.incidentService.createIncident(request).subscribe(
        response => {
          console.log('Incident created successfully:', response);
          // Reset the form after successful submission
          alert("incident declaré avec succés")
          this.incidentForm.reset();
        },
        error => {
          console.error('Error creating incident:', error),
          alert("Error creating incident:");}


      );
    }
  }
}
