import { Component } from '@angular/core';
import { NgStyle } from '@angular/common';
import { ReactiveFormsModule, FormsModule  } from '@angular/forms';
import { DocsExampleComponent } from '@docs-components/public-api';
import {ModalFooterComponent,
  ModalTitleDirective,
  ModalToggleDirective,
  PopoverDirective, ModalComponent,
  ModalHeaderComponent,
  ModalBodyComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, FormDirective, FormLabelDirective, FormControlDirective, ButtonDirective,InputGroupComponent,TableDirective, TableColorDirective, TableActiveDirective, BorderDirective, AlignDirective } from '@coreui/angular';
import { ContainerComponent, InputGroupTextDirective } from '@coreui/angular';
import {RoleService } from '../../_services/role.service';
import {UserService } from '../../_services/user.service';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-affect-role',
  standalone: true,
  imports: [ModalFooterComponent,
    ModalTitleDirective,
    ModalToggleDirective,
    PopoverDirective,ModalComponent,
    ModalHeaderComponent,
    ModalBodyComponent,RowComponent, CommonModule ,ContainerComponent,InputGroupTextDirective, ColComponent, TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, DocsExampleComponent, ReactiveFormsModule, FormsModule, FormDirective, FormLabelDirective, FormControlDirective, ButtonDirective, NgStyle,InputGroupComponent,TableDirective, TableColorDirective, TableActiveDirective, BorderDirective, AlignDirective],
  templateUrl: './affect-role.component.html',
  styleUrl: './affect-role.component.scss'
})
export class AffectRoleComponent {
  

  users: any[] = [];
  searchTerm: string = '';
  filteredUsers: any[] = [];
  roles: any[] = [];

  constructor(    private roleService: RoleService,
    private userService: UserService) { }

  ngOnInit(): void { 
    this.loadUsers();
    this.loadRoles();
  }

  loadUsers(): void {
    this.userService.getUsers().subscribe(
      (data) => {
        this.users = data;
        this.filteredUsers = data;
 
      },
      (error) => {
        console.error('Error fetching users:', error);
        // Handle error
      }
    );
  }

  

  searchUsers(): void {
    if (this.searchTerm) {
      this.filteredUsers = this.users.filter(user => 
        user.userName.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    } else {
      this.filteredUsers = this.users;
    }
  }

  loadRoles(): void {

    this.roleService.listRoles().subscribe(
      (data) => {
        this.roles = data;
      },
      (error) => {
        console.error('Error fetching roles:', error);
      }
    )
  }

  assignRole(user: any): void {
    console.log(user.Id , user.selectedRole)
    if (user.Id && user.selectedRole ) {

      this.roleService.assignRoleToUser(user.Id, user.selectedRole).subscribe(
        (response) => {
          if(response){
          console.log(`Role '${user.selectedRole}' assigned to user '${user.email}'.`);
          alert(`Role '${user.selectedRole}' assigned to user '${user.Email}'.`);}
        },
        (error) => {
          console.error('Error assigning role:', error);
          this.loadUsers();
        }
      );
    } else {
      alert('Please select a role to assign.');
    }
  }

 

  

}
