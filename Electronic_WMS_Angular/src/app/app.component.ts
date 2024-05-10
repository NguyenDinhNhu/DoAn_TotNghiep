import { Component } from '@angular/core';
import { CustomFunc } from '../assets/js/script.js';
import * as feather from 'feather-icons';
import { AuthAPIService } from './login/AuthAPIService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Electronic_WMS_Angular';
  public nameIdentifier!: number;

  ngOnInit(): void {
    feather.replace();
    this.nameIdentifier = this.getUserToken()['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    CustomFunc();
  }

  constructor(
    private router: Router,
    private authAPIService: AuthAPIService
  ) {}

  // Kiểm tra xem người dùng đã đăng nhập hay chưa
  isLoggedIn(): boolean {
    return this.authAPIService.isLoggedIn();
  }

  checkPermissions(): boolean {
    const token = this.authAPIService.getToken();
    if (token) {
      const parsedToken = this.parseJwt(token);
      const role = parsedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return role === 'Administrator' || role === 'Stocker';
    }
    return false;
  }

  isAdmin(): boolean {
    const token = this.authAPIService.getToken();
    if (token) {
      const parsedToken = this.parseJwt(token);
      const role = parsedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return role === 'Administrator';
    }
    return false;
  }

  isAdminOrStockerOrSupplier(): boolean {
    const token = this.authAPIService.getToken();
    if (token) {
      const parsedToken = this.parseJwt(token);
      const role = parsedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return role === 'Administrator' || role === 'Stocker' || role === 'Supplier';
    }
    return false;
  }

  isAdminOrStockerOrShop(): boolean {
    const token = this.authAPIService.getToken();
    if (token) {
      const parsedToken = this.parseJwt(token);
      const role = parsedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return role === 'Administrator' || role === 'Stocker' ||  role === 'Shop';
    }
    return false;
  }

  // Logout
  logOut(): void {
    this.authAPIService.clearToken(); // Xóa token khỏi sessionStorage
    this.router.navigate(['/login']); // Điều hướng đến trang login
  }

  getUserToken(): any {
    const token = this.authAPIService.getToken();
    if (token) {
      return this.parseJwt(token);
    } else {
      return null;
    }
  }

  parseJwt(token: string): any {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    return JSON.parse(window.atob(base64));
  }
}
