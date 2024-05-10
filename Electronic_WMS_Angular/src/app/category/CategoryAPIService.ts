import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthAPIService } from '../login/AuthAPIService';

@Injectable({
  providedIn: 'root'
})

export class CategoryAPIService {

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
  getListCategory(search: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Category/GetList', search, { headers });
  }

  getCategoryCombobox(): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Category/GetListCombobox', { headers });
  }

  getParentCategoryCombobox(): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Category/GetCategoryParentCombobox', { headers });
  }

  getCategory(id: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Category/GetCategory?id=' + id, { headers });
  }

  insertCategory(category: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Category/Insert', category, { headers });
  }

  updateCategory(category: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Category/Update', category, { headers });
  }

  deleteCategory(id: number): Observable<any> {
    const headers = this.getHeaders();
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Category/Delete', {}, {headers, params});
  }
}