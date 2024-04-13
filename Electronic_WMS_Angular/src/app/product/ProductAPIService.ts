import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class ProductAPIService {

  constructor(private http: HttpClient) { }

  getListProduct(search: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Product/GetList', search);
  }

  getListProductCombobox(): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Product/GetListCombobox');
  }

  getProduct(id: number): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Product/GetProduct?id=' + id);
  }

  insertProduct(product: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Product/Insert', product);
  }

  updateProduct(product: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Product/Update', product);
  }

  deleteProduct(id: number): Observable<any> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Product/Delete', {}, {params});
  }
}