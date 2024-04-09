import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class FeatureAPIService {

  constructor(private http: HttpClient) { }

  getListFeature(search: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Feature/GetList', search);
  }

  getFeatureCombobox(): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Feature/GetListCombobox');
  }

  getFeature(id: number): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Feature/GetFeature?id=' + id);
  }

  insertFeature(feature: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Feature/Insert', feature);
  }

  updateFeature(feature: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Feature/Update', feature);
  }

  deleteFeature(id: number): Observable<any> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Feature/Delete', {}, {params});
  }
}