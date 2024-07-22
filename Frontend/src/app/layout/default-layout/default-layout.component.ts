import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { NgScrollbar } from 'ngx-scrollbar';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { IconDirective } from '@coreui/icons-angular';
import {
  ContainerComponent,
  INavData,
  ShadowOnScrollDirective,
  SidebarBrandComponent,
  SidebarComponent,
  SidebarFooterComponent,
  SidebarHeaderComponent,
  SidebarNavComponent,
  SidebarToggleDirective,
  SidebarTogglerDirective
} from '@coreui/angular';

import { DefaultFooterComponent, DefaultHeaderComponent } from './';
import { navItems } from './_nav';
import { StorageService } from 'src/app/authentification/_AuthServices/storage.service';

function isOverflown(element: HTMLElement) {
  return (
    element.scrollHeight > element.clientHeight ||
    element.scrollWidth > element.clientWidth
  );
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss'],
  standalone: true,
  imports: [
    SidebarComponent,
    SidebarHeaderComponent,
    SidebarBrandComponent,
    RouterLink,
    IconDirective,
    NgScrollbar,
    SidebarNavComponent,
    SidebarFooterComponent,
    SidebarToggleDirective,
    SidebarTogglerDirective,
    DefaultHeaderComponent,
    ShadowOnScrollDirective,
    ContainerComponent,
    RouterOutlet,
    DefaultFooterComponent,
    FormsModule,
    ReactiveFormsModule
  ]  
})
export class DefaultLayoutComponent implements OnInit  {
  public navItems : INavData[] = navItems;

  constructor(private storageService: StorageService) {}
  ngOnInit(): void {
    this.navItems = this.filterNavItemsByRole(this.navItems,this.storageService.getUserRoles())
   // console.log( this.filterNavItemsByRole(this.navItems,this.storageService.getUserRoles()))
  }
  
  filterNavItemsByRole(navItems: INavData[], role: string[]): INavData[] {
    return navItems.filter(item => {
      return item.roles?.some(roles => role.includes(roles));
    }).map(item => {
      if (item.children) {
        item.children = this.filterNavItemsByRole(item.children, role);
      }
      return item;
    });
  }
  
  onScrollbarUpdate($event: any) {
    // if ($event.verticalUsed) {
    // console.log('verticalUsed', $event.verticalUsed);
    // }
  }

  
}
