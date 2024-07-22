import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { NgTemplateOutlet } from '@angular/common';
import {
  BorderDirective,
  ButtonDirective,
  CardBodyComponent,
  CardComponent,
  CardFooterComponent,
  CardGroupComponent,
  CardHeaderComponent,
  CardImgDirective,
  CardLinkDirective,
  CardSubtitleDirective,
  CardTextDirective,
  CardTitleDirective,
  ColComponent,
  GutterDirective,
  ListGroupDirective,
  ListGroupItemDirective,
  NavComponent,
  NavItemComponent,
  NavLinkDirective,
  RowComponent,
  TextColorDirective,
  ModalComponent,
  ModalHeaderComponent,
  ModalBodyComponent,
  TableModule,
  UtilitiesModule,
  ModalFooterComponent,
  ModalTitleDirective,
  ModalToggleDirective,
  PopoverDirective,
  ThemeDirective,
  TooltipDirective,
} from '@coreui/angular';
import { IncidentService } from '../../_services/incident.service';

import { StorageService } from 'src/app/authentification/_AuthServices/storage.service';

type CardColor = {
  color: string;
  textColor?: string;
};

@Component({
  selector: 'app-consulter-incident',
  standalone: true,
  imports: [
    ModalFooterComponent,
    ModalTitleDirective,
    ModalToggleDirective,
    PopoverDirective,
    ThemeDirective,
    TooltipDirective,
    TableModule,
    UtilitiesModule,
    ModalComponent,
    ModalHeaderComponent,
    ModalBodyComponent,
    RowComponent,
    CommonModule,
    ColComponent,
    TextColorDirective,
    CardComponent,
    CardHeaderComponent,
    CardBodyComponent,
    NgTemplateOutlet,
    CardTitleDirective,
    CardTextDirective,
    ButtonDirective,
    CardSubtitleDirective,
    CardLinkDirective,
    RouterLink,
    ListGroupDirective,
    ListGroupItemDirective,
    CardFooterComponent,
    NavComponent,
    NavItemComponent,
    NavLinkDirective,
    BorderDirective,
    CardGroupComponent,
    GutterDirective,
    CardImgDirective
  ],
  templateUrl: './consulter-incident.component.html',
  styleUrls: ['./consulter-incident.component.scss']
})
export class ConsulterIncidentComponent implements OnInit {
  colors: CardColor[] = [
    { color: 'primary', textColor: 'primary' },
    { color: 'secondary', textColor: 'secondary' },
    { color: 'success', textColor: 'success' },
    { color: 'danger', textColor: 'danger' },
    { color: 'warning', textColor: 'warning' },
    { color: 'info', textColor: 'info' },
    { color: 'light'},
    { color: 'dark'}
  ];

  incidents: any[] = [];
  userId: any = this.storageService.getUser()?.sub;
  statuts: any[] = [];
  priorites: any[] = [];
  types: any[] = [];
  incidentHistories: { [key: number]: any[] } = {};
  filteredIncidents: any[] = [];
  selectedIncidentHistory: any[] | null = null;
  selectedIncident: any = null;
  incidentDescription: any ;

  // Filter properties
  selectedStatus: number | null = null;
  selectedPriority: number | null = null;
  declarationDate: string | null = null;

  constructor(private incidentService: IncidentService, private storageService: StorageService) { }

  ngOnInit(): void {
    this.loadIncidents();
    this.loadInitialData();
    this.userId = this.storageService.getUser()?.sub;;
  }

  loadInitialData(): void {
    this.incidentService.getAllStatuts().subscribe(data => this.statuts = data);
    this.incidentService.getAllPriorites().subscribe(data => this.priorites = data);
    this.incidentService.getAllTypes().subscribe(data => this.types = data);
  }

  loadIncidents(): void {
    this.incidentService.getIncidentsByUser(this.userId).subscribe(
      (data) => {  
        this.incidents = data.map(item => item.Incident);
        console.log("before " + this.incidents)
         // Assign directly the incidents array
        this.applyFilters()
        console.log("after " + this.filteredIncidents)

      },
      (error) => {
        console.error('Error fetching incidents:', error);
      }
    );
  }
  
  

  applyFilters(): void {
    this.filteredIncidents = this.incidents.filter(incident => {
      const matchesStatus = this.selectedStatus === null || incident.INCD_STAT_ID === this.selectedStatus;
      const matchesPriority = this.selectedPriority === null || incident.INCD_PRIO_ID === this.selectedPriority;
      const matchesDate = this.declarationDate === null || new Date(incident.incD_DATE_DECLARATION).toDateString() === new Date(this.declarationDate).toDateString();
      return matchesStatus && matchesPriority && matchesDate;
    });
  }

  getStatutName(id: number): any {
    return this.statuts.find(s => s.INCD_STAT_ID === id);
  }

  getPrioriteName(id: number): any {
    return this.priorites.find(p => p.INCD_PRIO_ID === id);
  }

  getTypeName(id: number): any {
    return this.types.find(t => t.INCD_TYPE_ID === id);
  }

  loadIncidentHistory(incidentId: number): void {
    this.incidentService.getIncidentHistory(incidentId).subscribe(
      async (history) => {
        for (let entry of history) {
          try {
            const user = await this.incidentService.getUserInfos(entry.ChangedBy).toPromise();
            entry.userName = user.userName; // Ensure 'user.name' exists
          } catch {
            entry.userName = 'Unknown User'; // Handle errors gracefully
          }
        }
        this.incidentHistories[incidentId] = history;
        this.selectedIncidentHistory = history;
      },
      (error) => {
        console.error('Error fetching incident history:', error);
      }
    );
  }

  onViewDescription(incident: any): void {
    console.log(incident)
    this.incidentDescription = incident.INCD_DESC ;
  }

  onViewHistory(incidentId: number): void {
    this.loadIncidentHistory(incidentId);
  }

  onStatusChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.selectedStatus = target.value ? parseInt(target.value, 10) : null;
    this.applyFilters();
  }

  onPriorityChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.selectedPriority = target.value ? parseInt(target.value, 10) : null;
    this.applyFilters();
  }

  onDateChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.declarationDate = target.value || null;
    this.applyFilters();
  }

  UserInfos(userId : number): any{
    console.log(this.incidentService.getUserInfos(userId))
    return this.incidentService.getUserInfos(userId);
  }
}
