import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class InventoryAPIService {

  constructor(private http: HttpClient) { }

  getListInventoryByType(search: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Inventory/GetListByType', search);
  }

  getInventory(id: number): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Inventory/GetById?id=' + id);
  }

  insertInventory(inv: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Inventory/Insert', inv);
  }

  updateInventory(inv: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Inventory/Update', inv);
  }

  changeStatusInventory(inv: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Inventory/ChangeStatus', inv);
  }

  deleteInventory(id: number): Observable<any> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Inventory/Delete', {}, {params});
  }

  exportInventoryToPDF(id: number): Observable<Blob> {
    return this.http.get<Blob>(`http://localhost:5091/api/Inventory/ExportedPDFInventory?id=${id}`, { responseType: 'blob' as 'json' });
  }
}