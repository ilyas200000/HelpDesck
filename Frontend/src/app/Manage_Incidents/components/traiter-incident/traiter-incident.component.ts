import { Component, OnInit } from '@angular/core';
import { IncidentService } from '../../_services/incident.service';
import { FormsModule ,ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-traiter-incident',
  templateUrl: './traiter-incident.component.html',
  styleUrls: ['./traiter-incident.component.scss'],
  standalone:true,
  imports:[FormsModule , ReactiveFormsModule,CommonModule]
})
export class TraiterIncidentComponent implements OnInit {

  incidents: any[] = [];
  selectedIncident: any;
  selectedStatut: any ;
  statuts: any[] = [];
  userId!: number; 

  constructor(private incidentService: IncidentService) { }

  ngOnInit(): void {
    this.loadIncidents();
    this.fetchStatuts();
  }

  loadIncidents(): void {
    this.incidentService.getIncidentsByUser(this.userId).subscribe(
      (data) => {  
        this.incidents = data.map(item => item.Incident);
        console.log("Incidents récupérés:", this.incidents);
      },
      (error) => {
        console.error('Erreur lors de la récupération des incidents:', error);
      }
    );
  }

  fetchStatuts(): void {
    this.incidentService.getAllStatuts().subscribe(
      (data) => {
        this.statuts = data;
        console.log("Statuts récupérés:", this.statuts);
      },
      (error) => {
        console.error('Erreur lors de la récupération des statuts:', error);
      }
    );
  }

  handleChangeStatut(statutId: number): void {
    if (!this.selectedIncident) {
      console.error('Aucun incident sélectionné.');
      return;
    }

    this.incidentService.changeIncidentStatut(this.selectedIncident.INCD_ID, statutId, this.userId).subscribe(
      () => {
        console.log('Statut de l\'incident mis à jour avec succès.');
        // Rafraîchir la liste des incidents après la modification du statut
        this.loadIncidents();
        this.selectedIncident = null;
      },
      (error: any) => {
        console.error('Erreur lors du changement de statut de l\'incident:', error);
      }
    );
  }

}
