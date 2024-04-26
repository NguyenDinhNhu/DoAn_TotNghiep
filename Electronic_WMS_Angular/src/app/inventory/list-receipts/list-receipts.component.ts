import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import * as feather from 'feather-icons';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { InventoryAPIService } from '../InventoryAPIService';
import { ActivatedRoute, Router } from '@angular/router';
import { SerialNumberAPIService } from 'src/app/serial-number/SerialNumberAPIService';

@Component({
  selector: 'app-list-receipts',
  templateUrl: './list-receipts.component.html',
  styleUrls: ['./list-receipts.component.css']
})
export class ListReceiptsComponent {
  public pageSize: number = 10;
  public currentPage: number = 1;
  public textSearch!: string;
  public listReceipts: any[] = [];
  public totalItem: number = 0;
  public status: number = 0;

  public submited: boolean = false;
  public ListInventoryLine: any[] = [];
  public ListSerial: any[] = [];
  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private inventoryService: InventoryAPIService,
    private serialService: SerialNumberAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }


  ngOnInit(): void {
    feather.replace();
    this.getAllReceipts();
  }

  // delete
  deleteReceipt(id: number): void {
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
            this.getAllReceipts();
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

  completeReceipt(id: number): void {
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
            this.OpenModalEdit(id);
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

  cancelReceipt(id: number): void {
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
            this.getAllReceipts();
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

  getAllReceipts(): any {
    this.inventoryService.getListInventoryByType({
      PageSize: this.pageSize, 
      CurrentPage: this.currentPage, 
      TextSearch: this.textSearch,
      Type: 1,
      Status: 0}).subscribe(res => {
        this.listReceipts = res.listInventory;
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
      Type: 1,
      Status: this.status}).subscribe(res => {
      this.listReceipts = res.listInventory;
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
      Type: 1,
      Status: this.status}).subscribe(res => {
      this.listReceipts = res.listInventory;
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
      this.listReceipts = res.listInventory;
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
      Type: 1,
      Status: this.status}).subscribe(res => {
      this.listReceipts = res.listInventory;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }
  
  editLocation() : void {
    this.submited = true;
    
    let toastrSent = false; // Biến cờ để kiểm tra xem đã gửi toastr hay chưa
    let hasError = false;
    let hasDuplicateLocation = false;
    
    this.ListInventoryLine.forEach(row => {
      row.submited = true; // Đánh dấu rằng đã submit form
      const isInvalidRow = !row.ListSerialNumber.every(seri => {
        return seri.Location && seri.Location.trim() !== ''; // Kiểm tra tính hợp lệ của seri.Location
      });
      if (isInvalidRow && !toastrSent) {
        toastrSent = true; // Đánh dấu rằng đã gửi toastr
        hasError = true; // Đánh dấu rằng có lỗi được thông báo
        this.toastr.error('Please fill out all fields in each row of the list serial', 'Error');
      }

      let locations = new Set(); // Set để lưu trữ các location đã xuất hiện trong dòng

      // Duyệt qua từng location trong dòng hiện tại
      row.ListSerialNumber.forEach(serial => {
          // Nếu location đã xuất hiện trước đó trong cùng một dòng, đặt biến cờ là true
          if (locations.has(serial.Location)) {
              hasDuplicateLocation = true;
          } else {
              locations.add(serial.Location); // Thêm location vào set
          }
      });
    });

    // Nếu có location trùng nhau, hiển thị thông báo và không thực hiện hành động tiếp theo
    if (hasDuplicateLocation) {
      this.toastr.error('Duplicate locations are not allowed in the same row.', 'Error');
      return;
    }

    // Nếu không có lỗi nào được thông báo, thêm dữ liệu vào ListSerial
    if (!hasError) {
      this.ListSerial = [];
      this.ListInventoryLine.forEach(row => {
          row.ListSerialNumber.forEach(seri => {
            this.ListSerial.push({ SerialId: seri.SerialId, Location: seri.Location, WareHouseId: seri.WareHouseId});
          });
      });
      console.log(this.ListSerial)
      //edit
      this.serialService.updateLocation(
        this.ListSerial
      ).subscribe(res => {
        if(res.statusCode == 200){
          this.submited = false;
          // this.router.navigate(['/receipts/index']).then(() => {
          //   this.toastr.success(res.statusMessage, "Success");
          // });
          this.toastr.success(res.statusMessage, "Success");
          this.CloseModalEdit();
          this.getAllReceipts();
        }
        else if(res.statusCode == 404){
          this.toastr.warning(res.statusMessage, "Warning");
        }
        else if(res.statusCode == 400){
          this.toastr.warning(res.statusMessage, "Warning");
        }
        else if(res.statusCode == 500){
          this.toastr.error(res.statusMessage, "Error");
        }
      })
    }
    
  }
  OpenModalEdit(id: number) {
    const modelDiv = document.getElementById("editModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }
    this.inventoryService.getInventory(id).subscribe(res => {
      res.listInventoryLine.forEach(e => {
        const ListSeri: any[] = [];
        e.listSerialNumber.forEach(ele => {
          const r = {
            SerialId: ele.serialId,
            SerialNumber: ele.serialNumber,
            WareHouseId: ele.wareHouseId,
            Location: ele.location,
            submited: false
          }
          ListSeri.push(r)
        })
        const row = {
            InventoryLineId: e.inventoryLineId,
            ProductName: e.productName,
            Quantity: e.quantity,
            ListSerialNumber: ListSeri,
            submited: false,
          }
        this.ListInventoryLine.push(row)
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
