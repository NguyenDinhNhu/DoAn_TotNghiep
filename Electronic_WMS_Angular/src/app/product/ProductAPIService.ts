import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthAPIService } from '../login/AuthAPIService';

@Injectable({
  providedIn: 'root'
})

export class ProductAPIService {

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
  
  getListProduct(search: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Product/GetList', search, { headers });
  }

  getListProductStock(search: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Product/GetListProductStock', search, { headers });
  }

  getListProductCombobox(): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Product/GetListCombobox', { headers });
  }

  getProduct(id: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>('http://localhost:5091/api/Product/GetProduct?id=' + id, { headers });
  }

  insertProduct(product: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Product/Insert', product, { headers });
  }

  updateProduct(product: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<any>('http://localhost:5091/api/Product/Update', product, { headers });
  }

  deleteProduct(id: number): Observable<any> {
    const headers = this.getHeaders();
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Product/Delete', {}, {headers, params});
  }

  exportStockToExcel(): Observable<Blob> {
    const headers = this.getHeaders();
    return this.http.get<Blob>('http://localhost:5091/api/Product/ExportExcelStock', {headers, responseType: 'blob' as 'json' });
  }
}