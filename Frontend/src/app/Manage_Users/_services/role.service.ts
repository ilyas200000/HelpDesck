import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environment';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/authentification/_AuthServices/auth-service.service';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(private http: HttpClient, private authService: AuthService) { }

  createRole(roleName: string): Observable<any> {
    return this.http.post<any>(environment.apiUrlAuth + '/api/Roles/CreateRole', { roleName });
  }

  assignRoleToUser(idUser: string, idRole: string): Observable<any> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    const url = `${environment.apiUrlAuth}/api/Roles/AssignRole?idUser=${encodeURIComponent(idUser)}&idRole=${encodeURIComponent(idRole)}`;

    return this.http.post<any>(url, {}, { headers });
  }

  removeRoleFromUser(userEmail: string, roleName: string): Observable<any> {
    return this.http.post<any>(environment.apiUrlAuth + '/api/Roles/RemoveRole', { userEmail, roleName });
  }

  listRoles(): Observable<any> {
    const token = this.authService.getToken();
    return this.http.get<any>(environment.apiUrlAuth + '/api/Roles/ListRoles', {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      })
    });
  }

  getUserRoles(userEmail: string): Observable<any> {
    return this.http.get<any>(environment.apiUrlAuth + '/api/Roles/GetUserRoles?userEmail=' + userEmail);
  }

  assignDroitsToRole(roleId: number, droitIds: number[]): Observable<any> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    const url = `${environment.apiUrlAuth}/api/Roles/AssignDroitsToRole`;
    return this.http.post<any>(url, { roleId, droitIds }, { headers });
  }

  getDroits(): Observable<any> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<any>(environment.apiUrlAuth + '/api/Droits', { headers });
  }
}
