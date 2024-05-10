import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthAPIService } from './AuthAPIService';
import { ActivatedRoute, Router } from '@angular/router';
import * as feather from 'feather-icons';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  public submited: boolean = false;
  public UserName!: string;
  public Password!: string;

  loginForm = this.fb.group({
    UserName: ['', Validators.required],
    Password: ['', Validators.required]
  });

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private authAPIService: AuthAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { 
    // Kiểm tra nếu đã đăng nhập, điều hướng đến trang chính
    if (this.authAPIService.isLoggedIn()) {
      this.router.navigate(['/']);
    }
  }
  
  get f() {return this.loginForm.controls;}

  ngOnInit(): void {
    feather.replace();
  }

  // Login 
  onSubmit(): void {
    this.submited = true;
    
    if(!this.loginForm.invalid){
      console.log(this.loginForm.value);
      this.UserName = this.loginForm.value.UserName?.trim()!;
      this.Password = this.loginForm.value.Password?.trim()!;
      //call api login
      this.authAPIService.Login( { 
        UserName: this.UserName,
        PassWord: this.Password }).subscribe(res => {
          this.submited = false;
          this.toastr.success("Login Successfully!", "Success");
          sessionStorage.setItem('auth_token', res.token);
          this.router.navigate(['/']); // Điều hướng đến trang Dashboard
      }, error => {
        this.toastr.success(error.error, "Error");
      });
    }
  }
}
