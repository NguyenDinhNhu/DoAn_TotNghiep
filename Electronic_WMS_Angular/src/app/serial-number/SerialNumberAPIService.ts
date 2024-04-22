import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class SerialNumberAPIService {

  constructor(private http: HttpClient) { }

  getListSerialByProductId(search: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/SerialNumber/GetListByProductId', search);
  }

  getListSerialCombobox(search: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/SerialNumber/getListSerialCombobox',search);
  }

  updateLocation(seri: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/SerialNumber/UpdateLocation', seri);
  }
}