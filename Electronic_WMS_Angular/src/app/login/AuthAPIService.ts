import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})

export class AuthAPIService {

  private readonly TOKEN_KEY = 'auth_token';

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

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

  parseJwt(token: string): any {
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      return JSON.parse(window.atob(base64));
    } catch (error) {
      return null;
    }
  }

  isTokenExpired(token: string): boolean {
    const decoded = this.parseJwt(token);
    if (!decoded || !decoded.exp) {
      return true;
    }

    const expirationDate = decoded.exp * 1000;
    const currentDate = new Date().getTime();
    return expirationDate < currentDate;
  }

  checkTokenAndRedirect() {
    const token = this.getToken();
    if (token && this.isTokenExpired(token)) {
      sessionStorage.removeItem('auth_token');
      Swal.fire({
        title: 'Session Expired',
        text: 'Your session has expired. Please log in again.',
        icon: 'warning',
        confirmButtonText: 'OK'
      }).then(() => {
        this.router.navigate(['/login']);
      });
    }
  }
}