import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        data: {
            title: 'Incidents'
          },

        children: [
            {
                path: '',
                redirectTo: 'Incidents',
                pathMatch: 'full'
            },
            {
                path: 'declarer',
                loadComponent: () => import('./create-incident/create-incident.component').then(m => m.CreateIncidentComponent),
                data: {
                    title: "Declaration"
                }
            },
              {
                path: 'annuler',
                loadComponent: () => import('./annuler-incident/annuler-incident.component').then(m => m.AnnulerIncidentComponent),
                data: {
                  title: "Annulation "
                }
              },
              {
                path: 'consulter',
                loadComponent: () => import('./consulter-incident/consulter-incident.component').then(m => m.ConsulterIncidentComponent),
                data: {
                  title: "Consultation"
                }
              },
              {
                path: 'traiter',
                loadComponent: () => import('./traiter-incident/traiter-incident.component').then(m => m.TraiterIncidentComponent),
                data: {
                  title: "Traitement"
                }
              }

        ]
    }
];

