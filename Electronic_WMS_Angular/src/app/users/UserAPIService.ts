import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthAPIService } from '../login/AuthAPIService';

@Injectable({
  providedIn: 'root'
})

export class UserAPIService {

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
  getListUser(search: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Users/GetList', search, { headers });
  }

  getListSupplierOrShop(roleId: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Users/GetListSupplierOrShop', roleId, { headers });
  }

  getUser(id: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Users/GetUser?id=' + id, { headers });
  }

  insertUser(user: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Users/Insert', user, { headers });
  }

  updateUser(user: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Users/Update', user, { headers });
  }

  deleteUser(id: number): Observable<any> {
    const headers = this.getHeaders();
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Users/Delete', {}, {headers, params});
  }
}