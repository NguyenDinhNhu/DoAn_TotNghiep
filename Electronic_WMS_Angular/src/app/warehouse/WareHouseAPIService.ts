import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthAPIService } from '../login/AuthAPIService';

@Injectable({
  providedIn: 'root'
})

export class WareHouseAPIService {

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
  getListWareHouse(search: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/WareHouse/GetList', search, { headers });
  }

  getWareHouseCombobox(): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/WareHouse/GetListCombobox', { headers });
  }

  getWareHouse(id: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/WareHouse/GetWareHouse?id=' + id, { headers });
  }

  insertWareHouse(wh: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/WareHouse/Insert', wh, { headers });
  }

  updateWareHouse(wh: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/WareHouse/Update', wh, { headers });
  }

  deleteWareHouse(id: number): Observable<any> {
    const headers = this.getHeaders();
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/WareHouse/Delete', {}, {headers, params});
  }
}