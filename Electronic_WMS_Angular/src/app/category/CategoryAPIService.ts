import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class CategoryAPIService {

  constructor(private http: HttpClient) { }

  getListCategory(search: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Category/GetList', search);
  }

  getCategoryCombobox(): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Category/GetListCombobox');
  }

  getParentCategoryCombobox(): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Category/GetCategoryParentCombobox');
  }

  getCategory(id: number): Observable<any> {
    return this.http.get<any>('http://localhost:5091/api/Category/GetCategory?id=' + id);
  }

  insertCategory(category: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Category/Insert', category);
  }

  updateCategory(category: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Category/Update', category);
  }

  deleteCategory(id: number): Observable<any> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.patch<any>('http://localhost:5091/api/Category/Delete', {}, {params});
  }
}