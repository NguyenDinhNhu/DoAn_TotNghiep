import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthAPIService } from '../login/AuthAPIService';

@Injectable({
  providedIn: 'root'
})

export class BrandAPIService {

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

  getListBrand(search: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Brand/GetList', search, { headers });
  }

  getBrandCombobox(): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Brand/GetListCombobox', { headers });
  }

  getParentBrandCombobox(): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Brand/GetParentBrandCombobox', { headers });
  }

  getBrand(id: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Brand/GetBrand?id=' + id, { headers });
  }

  insertBrand(brand: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Brand/Insert', brand, { headers });
  }

  updateBrand(brand: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Brand/Update', brand, { headers });
  }

  deleteBrand(id: number): Observable<any> {
    const headers = this.getHeaders();
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Brand/Delete', {}, {headers, params});
  }
}