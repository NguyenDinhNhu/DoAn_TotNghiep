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
    CustomFunc();
    feather.replace();
    this.nameIdentifier = this.getUserToken()['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
  }

  constructor(
    private router: Router,
    private authAPIService: AuthAPIService
  ) {}

  // Kiểm tra xem người dùng đã đăng nhập hay chưa
  isLoggedIn(): boolean {
    return this.authAPIService.isLoggedIn();
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
