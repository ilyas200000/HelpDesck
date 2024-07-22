import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import {
  RowComponent, ColComponent, TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, FormControlDirective, FormDirective, FormLabelDirective, FormSelectDirective, FormCheckComponent, FormCheckInputDirective, FormCheckLabelDirective, ButtonDirective, ColDirective, InputGroupComponent, InputGroupTextDirective, ButtonCloseDirective,
  ModalBodyComponent,
  ModalComponent,
  ModalFooterComponent,
  ModalHeaderComponent,
  ModalTitleDirective,
  ModalToggleDirective,
  PopoverDirective,
  ThemeDirective,
  TooltipDirective,TableModule, UtilitiesModule
} from '@coreui/angular';
import { IncidentService } from '../../_services/incident.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-annuler-incident',
  standalone: true,
  imports: [
    RowComponent, ColComponent, TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent,
    FormControlDirective, ReactiveFormsModule, FormsModule, FormDirective, FormLabelDirective,
    FormSelectDirective, FormCheckComponent, FormCheckInputDirective, FormCheckLabelDirective,
    ButtonDirective, ColDirective, InputGroupComponent, InputGroupTextDirective, CommonModule,  ModalBodyComponent,
    ModalComponent,
    ModalFooterComponent,
    ModalHeaderComponent,
    ModalTitleDirective,
    ModalToggleDirective,
    PopoverDirective,
    ThemeDirective,
    TooltipDirective,
    ButtonCloseDirective,TableModule, UtilitiesModule
  ],
  templateUrl: './annuler-incident.component.html',
  styleUrl: './annuler-incident.component.scss'
})
export class AnnulerIncidentComponent implements OnInit {
  searchForm: FormGroup;
  incident: any | null = null;
  statuts: any[] = [];
  priorites: any[] = [];
  types: any[] = [] ;
  statut: any  = null;
  priorite: any  = null;
  type: any = null;


  constructor(private fb: FormBuilder, private incidentService: IncidentService) {
    this.searchForm = this.fb.group({
      ticketNumber: ['']
    });
  }
  ngOnInit(): void {
   this.loadInitialData();  
  }

  onSearch(): void {
    const ticketNumber = this.searchForm.value.ticketNumber;
    this.incidentService.getIncidentByNumTick(ticketNumber).subscribe(
      (data: any) => {
        this.incident = data;
        this.getPrioriteName(this.incident.INCD_ID)
        this.getTypeName(this.incident.INCD_TYPE_ID)
        this.getStatutName(this.incident.INCD_STAT_ID)
        console.log(this.getStatutName(this.incident.INCD_STAT_ID))
      },
      error => {
        console.error('Error fetching incident', error);
        this.incident = null;
      }
    );
  }

  loadInitialData(): void {
    this.incidentService.getAllStatuts().subscribe(data => this.statuts = data);
    this.incidentService.getAllPriorites().subscribe(data => this.priorites = data);
    this.incidentService.getAllTypes().subscribe(data => this.types = data);
  }

  onConfirmCancel(): void {
    if (this.incident) {
      this.incidentService.cancelIncident(this.incident.INCD_ID).subscribe(
        response => {
          console.log('Incident cancelled successfully', response);
          alert("Incident cancelled successfully")
          this.incident = null; // Clear the incident after cancellation
          
        },
        error => {
          console.error('Error cancelling incident', error);
          alert("Incident deja annulé")
        }
      );
    }
  }

  getStatutName(id: number): any {
    this.statut = this.statuts.find(s => s.INCD_STAT_ID === id);
    return this.statut;
  }

  getPrioriteName(id: number): any {
    this.priorite = this.priorites.find(p => p.INCD_PRIO_ID === id);
    return this.priorite;
  }

  getTypeName(id: number): any {
    this.type = this.types.find(t => t.INCD_TYPE_ID === id);
    return this.type;
  }

}
