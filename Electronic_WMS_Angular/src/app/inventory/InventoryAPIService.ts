import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthAPIService } from '../login/AuthAPIService';

@Injectable({
  providedIn: 'root'
})

export class InventoryAPIService {

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
  getDashBoardVM(): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Inventory/GetDashBoardVM', { headers });
  }

  getListInventoryByType(search: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Inventory/GetListByType', search, { headers });
  }

  getInventory(id: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Inventory/GetById?id=' + id, { headers });
  }

  insertInventory(inv: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Inventory/Insert', inv, { headers });
  }

  updateInventory(inv: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Inventory/Update', inv, { headers });
  }

  changeStatusInventory(inv: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Inventory/ChangeStatus', inv, { headers });
  }

  deleteInventory(id: number): Observable<any> {
    const headers = this.getHeaders();
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Inventory/Delete', {}, {headers, params});
  }

  exportInventoryToPDF(id: number): Observable<Blob> {
    const headers = this.getHeaders();
    return this.http.get<Blob>(`http://localhost:5091/api/Inventory/ExportedPDFInventory?id=${id}`, {headers, responseType: 'blob' as 'json' });
  }

  exportMoveHistoryToExcel(type: number): Observable<Blob> {
    const headers = this.getHeaders();
    return this.http.get<Blob>(`http://localhost:5091/api/Inventory/ExportExcelMoveHistory?type=${type}`, {headers, responseType: 'blob' as 'json' });
  }
}