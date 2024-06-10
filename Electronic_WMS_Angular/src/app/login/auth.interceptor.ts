import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { AuthAPIService } from './AuthAPIService';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authAPIService: AuthAPIService, private router: Router) {}

//   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//     const token = this.authAPIService.getToken();

//     if (token && this.authAPIService.isTokenExpired(token)) {
//       sessionStorage.removeItem('auth_token');
//       Swal.fire({
//         title: 'Session Expired',
//         text: 'Your session has expired. Please log in again.',
//         icon: 'warning',
//         confirmButtonText: 'OK'
//       }).then(() => {
//         this.router.navigate(['/login']);
//       });
//       return throwError('Session expired');
//     }

//     return next.handle(req).pipe(
//       catchError((error: HttpErrorResponse) => {
//         if (error.status === 401) {
//           sessionStorage.removeItem('auth_token');
//           Swal.fire({
//             title: 'Unauthorized',
//             text: 'Your session has expired. Please log in again.',
//             icon: 'warning',
//             confirmButtonText: 'OK'
//           }).then(() => {
//             this.router.navigate(['/login']);
//           });
//         }
//         return throwError(error);
//       })
//     );
//   }
intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authAPIService.getToken();
    let authReq = req;

    if (token) {
      authReq = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });
    }

    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          Swal.fire({
            title: 'Session Expired',
            text: 'Your session has expired. Please log in again.',
            icon: 'warning',
            confirmButtonText: 'OK'
          }).then(() => {
            this.authAPIService.clearToken();
            this.router.navigate(['/login']);
          });
        }
        return throwError(error);
      })
    );
  }
}
