import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  {
    name: 'Dashboard',
    url: '/dashboard',
    iconComponent: { name: 'cil-speedometer' },
    roles: ['Utilisateur Demandeur','Super Administrateur'],

  },
  {
    name: 'Incidents',
    url:'/Incidents',
    iconComponent: { name: 'cil-warning' },
    
    roles: ['Utilisateur Demandeur','Super Administrateur'],

    children: [
      {
      name: 'Declarer Incidents',
      url: '/Incidents/declarer',
      icon: 'nav-icon-bullet',
      roles: ['Utilisateur Demandeur','Super Administrateur']
    },
    {
      name: 'Consulter les incidents',
      url: '/Incidents/consulter',
      icon: 'nav-icon-bullet',
      
      roles: ['Utilisateur Demandeur','Super Administrateur']
    },
    {
      name: 'Annuler Incidents',
      url: '/Incidents/annuler',
      icon: 'nav-icon-bullet',
      
      roles: ['Utilisateur Demandeur','Super Administrateur']
    },
    {
      name: 'Traiter Incidents',
      url: '/Incidents/traiter',
      icon: 'nav-icon-bullet',
      
      roles: ['Utilisateur Demandeur','Super Administrateur']
    }
  ]
  },

  {
    name: 'Administration',
    iconComponent: { name: 'cil-settings' },
    url:'/Administration',
    roles: ['Super Administrateur'],
    children:[
      {
      name: 'Creation de compte',
      url: '/Administration/createUser',
      icon: 'nav-icon-bullet',
      
      roles: ['Super Administrateur']
    },
    {
      name: 'Affectation de Profile',
      url: 'Administration/affecterRole',
      icon: 'nav-icon-bullet',
      roles: ['Super Administrateur']
    },
    {
      name: 'Gestion de Profile',
      url: 'Administration/gestionRole',
      icon: 'nav-icon-bullet',
      roles: ['Super Administrateur']
    },
    {
      name: 'Affectation droit au profil',
      url: 'Administration/affectationDroit',
      icon: 'nav-icon-bullet',
      roles: ['Super Administrateur']
    },
    ]

  }
  
  
];
