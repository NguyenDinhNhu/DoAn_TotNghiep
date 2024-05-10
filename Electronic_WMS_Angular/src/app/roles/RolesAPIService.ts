import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthAPIService } from '../login/AuthAPIService';

@Injectable({
  providedIn: 'root'
})

export class RolesAPIService {

  constructor(private http: HttpClient,  private authService: AuthAPIService) { }

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    if (token) {
      return new HttpHeaders({
        'Authorization': `Bearer ${token}`
      });
    } else {
      return new HttpHeaders();
    }
  }

  getListRole(search: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Roles/GetList', search, { headers });
  }

  getRoleCombobox(): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Roles/GetListCombobox', { headers });
  }

  getRole(id: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Roles/GetRoles?id=' + id, { headers });
  }

  insertRole(role: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Roles/Insert', role, { headers });
  }

  updateRole(role: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Roles/Update', role, { headers });
  }

  deleteRole(id: number): Observable<any> {
    const headers = this.getHeaders();
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Roles/Delete', {}, {headers, params});
  }
}