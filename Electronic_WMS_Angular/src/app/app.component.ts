import { Component } from '@angular/core';
import { CustomFunc } from '../assets/js/script.js';
import * as feather from 'feather-icons';
import { AuthAPIService } from './login/AuthAPIService';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Electronic_WMS_Angular';
  public nameIdentifier!: number;
  private isAlertShown: boolean = false;

  constructor(
    private router: Router,
    private authAPIService: AuthAPIService
  ) {}

  ngOnInit(): void {
    feather.replace();
    this.nameIdentifier = this.getUserToken()['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    CustomFunc();
    // this.authAPIService.checkTokenAndRedirect();
    this.checkTokenPeriodically();
  }

  checkTokenPeriodically(): void {
    setInterval(() => {
      const token = this.authAPIService.getToken();
      if (token && this.authAPIService.isTokenExpired(token) && !this.isAlertShown) {
        this.isAlertShown = true;
        Swal.fire({
          title: 'Session Expired',
          text: 'Your session has expired. Please log in again.',
          icon: 'warning',
          confirmButtonText: 'OK'
        }).then(() => {
          this.authAPIService.clearToken();
          this.router.navigate(['/login']);
          this.isAlertShown = false; // Reset flag after handling
        });
      }
    }, 5000); // Kiểm tra mỗi 5 giây
  }

  // Kiểm tra xem người dùng đã đăng nhập hay chưa
  isLoggedIn(): boolean {
    return this.authAPIService.isLoggedIn();
  }

  checkPermissions(): boolean {
    const token = this.authAPIService.getToken();
    if (token) {
      const parsedToken = this.authAPIService.parseJwt(token);
      const role = parsedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return role === 'Administrator' || role === 'Stocker';
    }
    return false;
  }

  isAdmin(): boolean {
    const token = this.authAPIService.getToken();
    if (token) {
      const parsedToken = this.authAPIService.parseJwt(token);
      const role = parsedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return role === 'Administrator';
    }
    return false;
  }

  isAdminOrStockerOrSupplier(): boolean {
    const token = this.authAPIService.getToken();
    if (token) {
      const parsedToken = this.authAPIService.parseJwt(token);
      const role = parsedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return role === 'Administrator' || role === 'Stocker' || role === 'Supplier';
    }
    return false;
  }

  isAdminOrStockerOrShop(): boolean {
    const token = this.authAPIService.getToken();
    if (token) {
      const parsedToken = this.authAPIService.parseJwt(token);
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
      return this.authAPIService.parseJwt(token);
    } else {
      return null;
    }
  }
}
