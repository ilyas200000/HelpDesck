import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        data: {
            title: 'Administration'
          },

        children: [
            {
                path: '',
                redirectTo: 'Administration',
                pathMatch: 'full'
            },
            {
                path: 'createUser',
                loadComponent: () => import('./add-user/add-user.component').then(m => m.AddUserComponent),
                data: {
                    title: "Creation de compte"
                }
            },
              {
                path: 'affecterRole',
                loadComponent: () => import('./affect-role/affect-role.component').then(m => m.AffectRoleComponent),
                data: {
                  title: "Affectation de Profile"
                }
              },
              {
                path: 'gestionRole',
                loadComponent: () => import('./gestion-role/gestion-role.component').then(m => m.GestionRoleComponent),
                data: {
                  title: "Gestion de Profile "
                }
              },
              {
                path: 'affectationDroit',
                loadComponent: () => import('./affecter-droit/affecter-droit.component').then(m => m.AffecterDroitComponent),
                data: {
                  title: "Affectation droit au profil"
                }
              }

        ]
    }
];

