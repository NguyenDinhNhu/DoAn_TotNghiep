import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class WareHouseAPIService {

  constructor(private http: HttpClient) { }

  getListWareHouse(search: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/WareHouse/GetList', search);
  }

  getWareHouseCombobox(): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/WareHouse/GetListCombobox');
  }

  getWareHouse(id: number): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/WareHouse/GetWareHouse?id=' + id);
  }

  insertWareHouse(wh: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/WareHouse/Insert', wh);
  }

  updateWareHouse(wh: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/WareHouse/Update', wh);
  }

  deleteWareHouse(id: number): Observable<any> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/WareHouse/Delete', {}, {params});
  }
}