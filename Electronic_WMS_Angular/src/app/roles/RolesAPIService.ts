import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class RolesAPIService {

  constructor(private http: HttpClient) { }

  getListRole(search: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Roles/GetList', search);
  }

  getRoleCombobox(): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Roles/GetListCombobox');
  }

  getRole(id: number): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Roles/GetRoles?id=' + id);
  }

  insertRole(role: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Roles/Insert', role);
  }

  updateRole(role: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Roles/Update', role);
  }

  deleteRole(id: number): Observable<any> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Roles/Delete', {}, {params});
  }
}