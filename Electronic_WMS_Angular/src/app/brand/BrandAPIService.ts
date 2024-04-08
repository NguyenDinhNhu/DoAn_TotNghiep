import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class BrandAPIService {

  constructor(private http: HttpClient) { }

  getListBrand(search: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Brand/GetList', search);
  }

  getBrandCombobox(): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Brand/GetListCombobox');
  }

  getParentBrandCombobox(): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Brand/GetParentBrandCombobox');
  }

  getBrand(id: number): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Brand/GetBrand?id=' + id);
  }

  insertBrand(brand: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Brand/Insert', brand);
  }

  updateBrand(brand: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Brand/Update', brand);
  }

  deleteBrand(id: number): Observable<any> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Brand/Delete', {}, {params});
  }
}