import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AuthAPIService {

  private readonly TOKEN_KEY = 'auth_token';

  constructor(private http: HttpClient) { }

  Login(loginModel: any): Observable<any> {
    return this.http.post<any>('http://localhost:5091/api/Authentication/Login', loginModel);
  }
  
  getToken(): string | null {
    return sessionStorage.getItem('auth_token');
  }

  isLoggedIn(): boolean {
    // Kiểm tra xem token đã tồn tại trong session storage hay không
    return !!sessionStorage.getItem(this.TOKEN_KEY);
  }
  
  // Xóa token khỏi sessionStorage sau khi đăng xuất
  public clearToken(): void {
    sessionStorage.removeItem(this.TOKEN_KEY);
  }
}