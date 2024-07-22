import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environment';
import {Incident } from '../_models/incident-req/incident-req.module'
import { AuthService } from 'src/app/authentification/_AuthServices/auth-service.service';
import { StorageService } from 'src/app/authentification/_AuthServices/storage.service';
@Injectable({
  providedIn: 'root'
})
export class IncidentService {

  constructor(private http: HttpClient,private authService : AuthService,private storageService: StorageService) { }

  // Create a new incident
  createIncident(request: any): Observable<any> {
    const token = this.authService.getToken();
    console.log(token)
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    const url = `${environment.apiUrlIncident}/api/Incident/add`;
    return this.http.post<any>(url, request,{headers}); 
  }

  // Get a specific incident by ID
  getIncident(id: number): Observable<Incident> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    const url = `${environment.apiUrlIncident}/api/Incident/${id}`;
    return this.http.get<any>(url,{headers});
  }
  // Get all types for a specific category
getTypesByCategory(categoryId: number): Observable<any[]> {
  const token = this.authService.getToken();
  const headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  });
  const url = `${environment.apiUrlIncident}/api/Types/byCategory/${categoryId}`;
  return this.http.get<any[]>(url, { headers });
}


  // Get all incidents
  getAllIncidents(): Observable<any[]> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    const url = `${environment.apiUrlIncident}/api/Incident/all`;
    return this.http.get<any[]>(url,{headers});
  }

  // Get all Priorites
  getAllPriorites(): Observable<any[]> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    const url = `${environment.apiUrlIncident}/api/Priorites/all`;
    return this.http.get<any[]>(url, { headers });
  }

  // Get all Statuts
  getAllStatuts(): Observable<any[]> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    const url = `${environment.apiUrlIncident}/api/Statuts/all`;
    return this.http.get<any[]>(url, { headers });
  }

  // Get all Types
  getAllTypes(): Observable<any[]> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    const url = `${environment.apiUrlIncident}/api/Types/all`;
    return this.http.get<any[]>(url, { headers });
  }

  // Get all categories
getAllCategories(): Observable<any[]> {
  const token = this.authService.getToken();
  const headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  });
  const url = `${environment.apiUrlIncident}/api/Categories/all`;
  return this.http.get<any[]>(url, { headers });
}

 // cancel an incident by id
 cancelIncident(id: number): Observable<Incident> {
  const token = this.authService.getToken();
  const headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  });
  const url = `${environment.apiUrlIncident}/api/Incident/cancel/${id}/${this.storageService.getUser()?.sub}`;
  return this.http.put<any>(url,{headers});
}
 // get an incident by num_tickets
getIncidentByNumTick(num_tick: string): Observable<Incident> {
  const token = this.authService.getToken();
  const headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  });
  const url = `${environment.apiUrlIncident}/api/Incident/ByNumTicket?ticketNumber=${num_tick}`;
  return this.http.get<any>(url,{headers});
}

// Get incidents by entity ID
getIncidentsByUser(userId: number): Observable<any[]> {
  const token = this.authService.getToken();
  const headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  });
  const url = `${environment.apiUrlIncident}/api/Incident/user/${userId}`;
  return this.http.get<any[]>(url, { headers });
}

// Add this method in the IncidentService class
getIncidentHistory(incidentId: number): Observable<any[]> {
  const token = this.authService.getToken();
  const headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  });
  const url = `${environment.apiUrlIncident}/api/Incident/${incidentId}/history`;
  return this.http.get<any[]>(url, { headers });
}

getUserInfos(userId: number): Observable<any> {
  const token = this.authService.getToken();
  const headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  });
  const url = `${environment.apiUrlAuth}/api/User/${userId}`;
  return this.http.get<any>(url, { headers });
}

// Modifier le statut d'un incident et retourner un Observable
changeIncidentStatut(idIncident: number, idStatut: number, idUser: number): Observable<any> {
  const token = this.authService.getToken();
  const headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  });
  const url = `${environment.apiUrlIncident}/api/Incident/Traiter`;
  const body = {
    id: idIncident,
    idStatut: idStatut,
    idUser: idUser
  };
  return this.http.post<any>(url, body, { headers });
}






}
