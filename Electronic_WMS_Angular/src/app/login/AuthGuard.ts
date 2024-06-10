import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthAPIService } from './AuthAPIService';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthAPIService, private router: Router) {}

  canActivate(): boolean {
    const token = this.authService.getToken();
    if (token && !this.authService.isTokenExpired(token)) {
      return true;
    } else {
      if (!token) {
        this.router.navigate(['/login']);
      } else {
        this.authService.checkTokenAndRedirect();
      }
      return false;
    }
  }
  // constructor(private router: Router) {}

  // canActivate(): boolean {
  //   if (this.isLoggedIn()) {
  //     return true;
  //   } else {
  //     this.router.navigate(['/login']);
  //     return false;
  //   }
  // }

  // isLoggedIn(): boolean {
  //   // Kiểm tra xem token đã tồn tại trong session storage hay không
  //   return !!sessionStorage.getItem('auth_token');
  // }
}