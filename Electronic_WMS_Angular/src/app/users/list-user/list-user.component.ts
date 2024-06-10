import { Component } from '@angular/core';
import { UserAPIService } from '../UserAPIService';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as feather from 'feather-icons';
import Swal from 'sweetalert2';
import { AuthAPIService } from 'src/app/login/AuthAPIService';

@Component({
  selector: 'app-list-user',
  templateUrl: './list-user.component.html',
  styleUrls: ['./list-user.component.css']
})
export class ListUserComponent {
  // Search - Get List 
  public pageSize: number = 10;
  public currentPage: number = 1;
  public textSearch!: string;
  public listUser: any[] = [];
  public totalItem: number = 0;


  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private userService: UserAPIService,
    private authAPIService: AuthAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

 
  ngOnInit(): void {
    feather.replace();
    this.getAllUser();
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

  // delete
  deleteUser(id: number): void {
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Confirm"
    }).then((result) => {
      if (result.isConfirmed) {
        this.userService.deleteUser(id).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.getAllUser();
          }
          else if (res.statusCode = 404){
            this.toastr.warning(res.statusMessage, "Warning");
          }
          else if (res.statusCode = 500) {
            this.toastr.error(res.statusMessage, "Error");
          }
        })
      }
    });
  }

  getAllUser(): any {
    this.userService.getListUser({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listUser = res.listUser;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    }, error => {
      this.toastr.error("Loading data fail!", "Error");
    })
  }

  // pagination
  changePage(page: number): void{
    this.currentPage = page;    
    console.log(this.currentPage);
    this.userService.getListUser({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listUser = res.listUser;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  onChangePageSize(event: any) {
    this.pageSize = event.target.value;
    this.userService.getListUser({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listUser = res.listUser;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  search(): void {
    this.userService.getListUser({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch.trim()}).subscribe(res => {
      this.listUser = res.listUser;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }
}
