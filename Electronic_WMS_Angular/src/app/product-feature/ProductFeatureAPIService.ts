import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class ProductFeatureAPIService {

  constructor(private http: HttpClient) { }

  getListProductFeature(search: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/ProductFeature/GetList', search);
  }

  getListByProductId(id: number): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/ProductFeature/GetListProductFeature?productId=' + id);
  }

  getProductFeature(id: number): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/ProductFeature/GetPFById?id=' + id);
  }

  insertProductFeature(product: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/ProductFeature/Insert', product);
  }

  updateProductFeature(product: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/ProductFeature/Update', product);
  }

  deleteProductFeature(id: number): Observable<any> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/ProductFeature/Delete', {}, {params});
  }
}