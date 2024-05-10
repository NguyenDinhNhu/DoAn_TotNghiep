import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { WareHouseAPIService } from './WareHouseAPIService';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import * as feather from 'feather-icons';
import Swal from 'sweetalert2';
import { AuthAPIService } from '../login/AuthAPIService';

@Component({
  selector: 'app-warehouse',
  templateUrl: './warehouse.component.html',
  styleUrls: ['./warehouse.component.css']
})
export class WarehouseComponent {
  // Search - Get List 
  public pageSize: number = 10;
  public currentPage: number = 1;
  public textSearch!: string;
  public listWareHouse: any[] = [];
  public totalItem: number = 0;

  public wareHouseName!: string;
  public address!: string;
  public wareHouseId!: number;
  // Form add 
  public submitedAdd: boolean = false;

  addWHForm = this.fb.group({
    WareHouseName: ['', Validators.required],
    Address: ['', Validators.required]
  });

  // Form edit
  public submitedEdit: boolean = false;

  editWHForm = this.fb.group({
    WareHouseId: ['', Validators.required],
    WareHouseName: ['', Validators.required],
    Address: ['', Validators.required]
  });

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private whService: WareHouseAPIService,
    private authAPIService: AuthAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  get f() {return this.addWHForm.controls;}
  get v() {return this.editWHForm.controls;}

  ngOnInit(): void {
    feather.replace();
    this.getAllWareHouse();
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

  parseJwt(token: string): any {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    return JSON.parse(window.atob(base64));
  }

  // Add 
  addWareHouse(): void {
    this.submitedAdd = true;
    
    if(!this.addWHForm.invalid){
      console.log(this.addWHForm.value);
      this.wareHouseName = this.addWHForm.value.WareHouseName!.trim();
      this.address = this.addWHForm.value.Address!.trim();
      //call api add 
      this.whService.insertWareHouse( 
      { Name: this.wareHouseName,
       Address: this.address}).subscribe(res => {
        if(res.statusCode == 200){
          this.toastr.success(res.statusMessage, "Success");
          this.submitedAdd = false;
          this.getAllWareHouse();
          this.CloseModal();
        }
        else if (res.statusCode = 400){
          this.toastr.warning(res.statusMessage, "Warning");
        }
        else if (res.statusCode = 500) {
          this.toastr.error(res.statusMessage, "Error");
        }
      });
    }
  }

  // Edit 
  editWareHouse(): void {
    this.submitedEdit = true;
    
    if(!this.editWHForm.invalid){
      console.log(this.editWHForm.value);
      this.wareHouseId = parseInt(this.editWHForm.value.WareHouseId!);
      this.wareHouseName = this.editWHForm.value.WareHouseName?.trim()!;
      this.address = this.editWHForm.value.Address?.trim()!;
      //call api update 
      this.whService.updateWareHouse( { 
        WareHouseId: this.wareHouseId,
        Name: this.wareHouseName,
        Address: this.address }).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.submitedAdd = false;
            this.getAllWareHouse();
            this.CloseModalEdit();
          }
          else if (res.statusCode = 400){
            this.toastr.warning(res.statusMessage, "Warning");
          }
          else if (res.statusCode = 404){
            this.toastr.warning(res.statusMessage, "Warning");
          }
          else if (res.statusCode = 500) {
            this.toastr.error(res.statusMessage, "Error");
          }
      });
    }
  }

  // delete
  deleteWareHouse(id: number): void {
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
        this.whService.deleteWareHouse(id).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.getAllWareHouse();
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

  getAllWareHouse(): any {
    this.whService.getListWareHouse({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listWareHouse = res.listWareHouse;
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
    this.whService.getListWareHouse({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listWareHouse = res.listWareHouse;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  onChangePageSize(event: any) {
    this.pageSize = event.target.value;
    this.whService.getListWareHouse({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listWareHouse = res.listWareHouse;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  search(): void {
    this.whService.getListWareHouse({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch.trim()}).subscribe(res => {
      this.listWareHouse = res.listWareHouse;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  // Popup Modal Add Brand  
  OpenModal() {
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }
    this.addWHForm.reset();
  }

  CloseModal() {
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "none";
    }
  }

  OpenModalEdit(id: number) {
    const modelDiv = document.getElementById("editModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }

    this.whService.getWareHouse(id).subscribe(res => {
      this.editWHForm = this.fb.group({
        WareHouseId: [res.wareHouseId, Validators.required],
        WareHouseName: [res.name, Validators.required],
        Address: [res.address, Validators.required]
      });
    })
  }

  CloseModalEdit() {
    const modelDiv = document.getElementById("editModal");
    if(modelDiv != null) {
      modelDiv.style.display = "none";
    }
  }
}
