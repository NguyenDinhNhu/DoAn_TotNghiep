import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class UserAPIService {

  constructor(private http: HttpClient) { }

  getListUser(search: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Users/GetList', search);
  }

  getListSupplierOrShop(): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Users/GetListSupplierOrShop');
  }

  getUser(id: number): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Users/GetUser?id=' + id);
  }

  insertUser(user: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Users/Insert', user);
  }

  updateUser(user: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Users/Update', user);
  }

  deleteUser(id: number): Observable<any> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Users/Delete', {}, {params});
  }
}