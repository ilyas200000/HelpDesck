import { Component } from '@angular/core';
import { NgStyle } from '@angular/common';
import { ReactiveFormsModule, FormsModule, Validators, FormGroup, FormBuilder } from '@angular/forms';
import { DocsExampleComponent } from '@docs-components/public-api';
import { RowComponent, ColComponent, TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, FormDirective, FormLabelDirective, FormControlDirective, ButtonDirective,InputGroupComponent,TableDirective, TableColorDirective, TableActiveDirective, BorderDirective, AlignDirective } from '@coreui/angular';
import { ContainerComponent, InputGroupTextDirective } from '@coreui/angular';
import {RoleService } from '../../_services/role.service';
import {UserService } from '../../_services/user.service';
import { AuthService } from 'src/app/authentification/_AuthServices/auth-service.service';
import { CommonModule } from '@angular/common';
@Component({
    selector: 'app-add-user',
    standalone: true,
    templateUrl: './add-user.component.html',
    styleUrl: './add-user.component.scss',
    imports: [RowComponent, CommonModule ,ContainerComponent,InputGroupTextDirective, ColComponent, TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, DocsExampleComponent, ReactiveFormsModule, FormsModule, FormDirective, FormLabelDirective, FormControlDirective, ButtonDirective, NgStyle,InputGroupComponent,TableDirective, TableColorDirective, TableActiveDirective, BorderDirective, AlignDirective]
})
export class AddUserComponent {
  addUserForm!: FormGroup;
  roles: any[] = [];
  favoriteColor = '#26ab3c';
  users: any[] = [];
  searchTerm: string = '';
  filteredUsers: any[] = [];

  constructor(
    private fb: FormBuilder,
    private roleService: RoleService,
    private userService: UserService
    
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.loadUsers();
    // this.loadRoles();
  }

  private initializeForm(): void {
    this.addUserForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      matricule: ['', Validators.required]
    });
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

  onSubmit(): void {
    if (this.addUserForm.valid) {
      const userData = this.addUserForm.value;
      this.userService.register(userData).subscribe({
        next: (response) => {
          // Handle successful response
          alert('user registred succesfully');
          this.loadUsers();
        },
        error: (error) => {
          // Show a user-friendly error message
          console.error('Failed to register user. Please try again.', error);
          alert('error');
        }
      });
    } else {
      // Handle form validation errors
      console.error('Form is invalid');
      alert('Please fill out the form correctly.');
    }
  }

  deleteUser(userId: string): void {
    this.userService.deleteUser(userId).subscribe(
      () => {
        console.error('Error deleting user');
      },
      error => {
        this.loadUsers(); // Reload users after deletion
        alert("user deleted succesfully")
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

}
