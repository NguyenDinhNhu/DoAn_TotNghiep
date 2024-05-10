import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import * as feather from 'feather-icons';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { InventoryAPIService } from '../InventoryAPIService';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthAPIService } from 'src/app/login/AuthAPIService';

@Component({
  selector: 'app-list-deliveries',
  templateUrl: './list-deliveries.component.html',
  styleUrls: ['./list-deliveries.component.css']
})
export class ListDeliveriesComponent {
  public pageSize: number = 10;
  public currentPage: number = 1;
  public textSearch!: string;
  public listDeliveries: any[] = [];
  public totalItem: number = 0;
  public status: number = 0;

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private inventoryService: InventoryAPIService,
    private authAPIService: AuthAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }


  ngOnInit(): void {
    feather.replace();
    this.getAllDeliveries();
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

  parseJwt(token: string): any {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    return JSON.parse(window.atob(base64));
  }

  // delete
  deleteDelivery(id: number): void {
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
        this.inventoryService.deleteInventory(id).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.getAllDeliveries();
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

  completeDelivery(id: number): void {
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
        this.inventoryService.changeStatusInventory({
          InventoryId: id,
          Status: 2
        }).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.getAllDeliveries();
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

  cancelDelivery(id: number): void {
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
        this.inventoryService.changeStatusInventory({
          InventoryId: id,
          Status: 3
        }).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.getAllDeliveries();
          }
          else if (res.statusCode = 404) {
            this.toastr.warning(res.statusMessage, "Warning");
          }
          else if (res.statusCode = 500) {
            this.toastr.error(res.statusMessage, "Error");
          }
        })
      }
    });
  }

  exportPDFInventory(id: number): void {
    this.inventoryService.exportInventoryToPDF(id).subscribe(
      (data: Blob) => {

        const now = new Date();
        const dateTimeStr = now.toISOString().slice(0, 19).replace(/[-T:/]/g, ""); // Format: YYYYMMDDHHmmss
        const fileName = `invoice_${dateTimeStr}.pdf`; // Tạo tên file với ngày giờ
  
        // Tạo một URL tạm thời từ dữ liệu Blob nhận được
        const url = window.URL.createObjectURL(data);
        // Tạo một đối tượng a để tạo một liên kết trực tiếp đến file PDF
        const link = document.createElement('a');
        link.href = url;
        link.download = fileName; // Tên file PDF khi được tải xuống
        // Thêm liên kết vào DOM và kích hoạt sự kiện click để tải xuống file PDF
        document.body.appendChild(link);
        link.click();
        // Xóa liên kết và URL tạm thời sau khi đã tải xuống xong
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
      },
      (error) => {
        this.toastr.error("Exported fail!", "Error");
      }
    );
  }

  getAllDeliveries(): any {
    this.inventoryService.getListInventoryByType({
      PageSize: this.pageSize, 
      CurrentPage: this.currentPage, 
      TextSearch: this.textSearch,
      Type: 2,
      Status: 0}).subscribe(res => {
        this.listDeliveries = res.listInventory;
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
    this.inventoryService.getListInventoryByType({
      PageSize: this.pageSize, 
      CurrentPage: this.currentPage, 
      TextSearch: this.textSearch,
      Type: 2,
      Status: this.status}).subscribe(res => {
      this.listDeliveries = res.listInventory;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  onChangePageSize(event: any) {
    this.pageSize = event.target.value;
    this.inventoryService.getListInventoryByType({
      PageSize: this.pageSize, 
      CurrentPage: this.currentPage,
      TextSearch: this.textSearch,
      Type: 2,
      Status: this.status}).subscribe(res => {
      this.listDeliveries = res.listInventory;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  filterByStatus(event: any) {
    this.status = event.target.value;
    this.inventoryService.getListInventoryByType({
      PageSize: this.pageSize, 
      CurrentPage: this.currentPage,
      TextSearch: this.textSearch,
      Type: 1,
      Status: this.status}).subscribe(res => {
      this.listDeliveries = res.listInventory;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  search(): void {
    this.inventoryService.getListInventoryByType({
      PageSize: this.pageSize, CurrentPage: this.currentPage, 
      TextSearch: this.textSearch.trim(),
      Type: 2,
      Status: this.status}).subscribe(res => {
      this.listDeliveries = res.listInventory;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }
}
