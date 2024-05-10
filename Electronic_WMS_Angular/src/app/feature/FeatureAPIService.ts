import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthAPIService } from '../login/AuthAPIService';

@Injectable({
  providedIn: 'root'
})

export class FeatureAPIService {

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
  
  getListFeature(search: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Feature/GetList', search, { headers });
  }

  getFeatureCombobox(): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Feature/GetListCombobox', { headers });
  }

  getFeature(id: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Feature/GetFeature?id=' + id, { headers });
  }

  insertFeature(feature: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Feature/Insert', feature, { headers });
  }

  updateFeature(feature: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Feature/Update', feature, { headers });
  }

  deleteFeature(id: number): Observable<any> {
    const headers = this.getHeaders();
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Feature/Delete', {}, {headers, params});
  }
}