import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthAPIService } from '../login/AuthAPIService';

@Injectable({
  providedIn: 'root'
})

export class SerialNumberAPIService {

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

  getListSerialByProductId(search: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/SerialNumber/GetListByProductId', search, { headers });
  }

  getListSerialCombobox(search: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/SerialNumber/getListSerialCombobox',search, { headers });
  }

  updateLocation(seri: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/SerialNumber/UpdateLocation', seri, { headers });
  }

  getSerial(id: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/SerialNumber/GetSerial?id=' + id, { headers });
  }
}