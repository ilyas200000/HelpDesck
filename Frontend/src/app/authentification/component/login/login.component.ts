import { Component, OnInit } from '@angular/core';
import { NgStyle } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, CardGroupComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective } from '@coreui/angular';
import { AuthService } from '../../_AuthServices/auth-service.service';
import { FormsModule,NgForm,ReactiveFormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import {Router} from "@angular/router";

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, CardGroupComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective, NgStyle,FormsModule,ReactiveFormsModule]
})
export class LoginComponent implements OnInit {
  errorMessage! : string;
  AuthUserSub! : Subscription;
  constructor(private authService : AuthService, private router : Router) { }

  ngOnInit(): void {
    this.AuthUserSub = this.authService.AuthenticatedUser$.subscribe({
      next : user => {
        if(user != null) {
          this.router.navigate(['/dashboard']);
          console.log(user)
        }
      }
    })
  }


  //  username: string = '';
  //  password: string = '';

   login(formLogin: NgForm) {
    // console.log(this.username);
     if(!formLogin.valid){
      return;
    }
    const username = formLogin.value.username;
    const password = formLogin.value.password;

    this.authService.login(username, password).subscribe({
      next : userData => {
        this.router.navigate(['dashboard']);
      },
      error : err => {
        this.errorMessage = err;
        console.log(err);
        alert("username or password invalid!")
      }

    })
  }
}
