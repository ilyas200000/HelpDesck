import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import {environment} from '../../../../environment';
import { BehaviorSubject, catchError, mergeMap, tap, throwError } from 'rxjs';
import {StorageService} from '../_AuthServices/storage.service';
import { User } from '../_models/user/user.module';
import { Router } from '@angular/router';

export interface AuthResponseData {
  access_token: string;
  expires_in: number;
  token_type: string;
  scope: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private storageService : StorageService;
  AuthenticatedUser$ = new BehaviorSubject<User | null>(null);

  constructor( private http: HttpClient,private router: Router) {
    this.storageService = inject(StorageService);
  }

  login(username: string, password: string) {
    const requestBody = `grant_type=password&username=${encodeURIComponent(username)}&password=${encodeURIComponent(password)}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/x-www-form-urlencoded',
      'Authorization': 'Basic ' + btoa(`${environment.clientId}:${environment.clientSecret}`)
    });
   
  
    return this.http.post<AuthResponseData>(`${environment.apiUrlAuth}/connect/token`, requestBody,{ headers }).pipe(
      mergeMap(response => {
        if (response && response.access_token) {
          // Token retrieved, save it or perform further actions
           this.storageService.saveAuthResponseData(response);
        } else {
          // Handle missing token in response
          throw new Error('Access token not found in response');
          this.cleanAndNavigate();
        }
        // After getting the access token, make another request to get user info
        return this.http.get<User>(`${environment.apiUrlAuth}/connect/userinfo`, {
          headers: {
            Authorization: `Bearer ${response.access_token}`
          }
        });
      }),
      catchError(err => {
        console.log(err);
        let errorMessage = 'An unknown error occurred!';
        if (err.error.message === 'Bad credentials') {
          errorMessage = 'The email address or password you entered is invalid';
          
        }
        return throwError(() => new Error(errorMessage));
      }),
      tap(user => {
        // Save user information or perform further actions
        this.storageService.saveUser(user);
        this.AuthenticatedUser$.next(user);
      
      })
    );
  }

  

  getToken(): string | null {
    try {
      const authResponseData = window.localStorage.getItem('access-token');
      if (authResponseData) {
        const parsedData: AuthResponseData = JSON.parse(authResponseData);
        return parsedData.access_token;
      }
      return null;
    } catch (error) {
      console.error('Error retrieving token from localStorage', error);
      return null;
    }
  }
  autoLogin() {
    const userData = this.storageService.getUser();
    if (userData) {
      this.AuthenticatedUser$.next(userData);
    }
  }

  getIdToken(): string | null {
    const authResponseData = window.localStorage.getItem('authResponse');
    if (authResponseData) {
      const parsedData = JSON.parse(authResponseData);
      return parsedData.id_token;
    }
    return null;
  }
  

  logout(): void {
    const idToken = this.getIdToken();
    if (!idToken) {
        this.cleanAndNavigate();
        return;
    }

    const logoutUrl = `${environment.apiUrlAuth}/connect/endsession`;
    const requestBody = `id_token_hint=${idToken}&post_logout_redirect_uri=${encodeURIComponent(window.location.origin + '/login')}`;
    const headers = new HttpHeaders({
        'Content-Type': 'application/x-www-form-urlencoded'
    });

    this.http.post(logoutUrl, requestBody, { headers }).subscribe({
        next: () => {
            // Successful logout, clean storage and navigate to login page
            this.cleanAndNavigate();
        },
        error: (err) => {
            console.error('Logout error', err);
            // Handle any error during logout and still clean storage and navigate
            this.cleanAndNavigate();
        }
    });
}
returnLogin():void {
      if(this.AuthenticatedUser$=== null || this.AuthenticatedUser$ == undefined){
        this.router.navigate(['/login']);
      }
}
private cleanAndNavigate() {
    this.storageService.clean();
    this.AuthenticatedUser$.next(null);
    this.router.navigate(['/login']);
}

  
}