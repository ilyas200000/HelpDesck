import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environment';
import { AuthService } from 'src/app/authentification/_AuthServices/auth-service.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient :HttpClient,private authService : AuthService) { }

  register(userData: any): Observable<any> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    return this.httpClient.post<any>(environment.apiUrlAuth + '/api/user/register', userData, { headers });
  }
  getUsers(): Observable<any[]> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    return this.httpClient.get<any[]>(environment.apiUrlAuth + '/api/user/all', { headers });
  }

  deleteUser(userId: string): Observable<any> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    return this.httpClient.delete<any>(`${environment.apiUrlAuth}/api/user/delete/${userId}`, { headers });
  }
}
