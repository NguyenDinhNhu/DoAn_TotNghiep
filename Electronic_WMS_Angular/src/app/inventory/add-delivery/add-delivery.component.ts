import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import * as feather from 'feather-icons';
import { ToastrService } from 'ngx-toastr';
import { InventoryAPIService } from '../InventoryAPIService';
import { UserAPIService } from 'src/app/users/UserAPIService';
import { WareHouseAPIService } from 'src/app/warehouse/WareHouseAPIService';
import { ProductAPIService } from 'src/app/product/ProductAPIService';
import { ActivatedRoute, Router } from '@angular/router';
import { SerialNumberAPIService } from 'src/app/serial-number/SerialNumberAPIService';

@Component({
  selector: 'app-add-delivery',
  templateUrl: './add-delivery.component.html',
  styleUrls: ['./add-delivery.component.css']
})
export class AddDeliveryComponent {
  public submited: boolean = false;
  public SourceLocation!: number;
  public WarehouseId!: number;

  // table edit
  public ListInventoryLine: any[] = []; 
  public Quantity!: number;
  public ProductId!: number;

  public ListSerial: any[] = []; 
  public SerialId!: number;

  public ListRow: any[] = [];
  // Combobox
  public ListShopCombobox: any[] = [];
  public ListWHCombobox: any[] = [];
  public ListSerialCombobox: any[] = [];
  public ListProductCombobox: any[] = [];
  public ListSerialComboboxByWH: any[] = [];

  addDelivery = this.fb.group({
    SourceLocation: ['', Validators.required],
    WareHouseId: ['', Validators.required]
  });
  
  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private inventoryService: InventoryAPIService,
    private userService: UserAPIService,
    private warehouseService: WareHouseAPIService,
    private productService: ProductAPIService,
    private serialService: SerialNumberAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    feather.replace();
    this.getListWHCombobox();
    this.getListCateCombobox();
    this.getListUserCombobox();

    this.addDelivery.get('WareHouseId')!.valueChanges.subscribe((warehouseId: any) => {
      console.log(warehouseId)
      this.onWarehouseChange(warehouseId);
    });
    // Khởi tạo dữ liệu hoặc lấy từ API nếu cần thiết
    this.updateProductQuantities();

  }

  onWarehouseChange(warehouseId: number) {
    if (warehouseId) {
      // Xóa tất cả các dòng trong bảng
      this.ListRow = [];
      this.updateProductQuantities();

      this.serialService.getListSerialComboboxByWH(warehouseId).subscribe(res => {
        this.ListSerialComboboxByWH = res;
        console.log(res)
        console.log(this.ListSerialComboboxByWH)
      });
    }
  }

  // Get List combobox Supplier
  getListUserCombobox(): void {
    this.userService.getListSupplierOrShop(4).subscribe(res => {
      this.ListShopCombobox = res;
    })
  }

  getListWHCombobox(): void {
    this.warehouseService.getWareHouseCombobox().subscribe(res => {
      this.ListWHCombobox = res;
    })
  }

  getListCateCombobox(): void {
    this.productService.getListProductCombobox().subscribe(res => {
      this.ListProductCombobox = res;
    })
  }

  get f() {return this.addDelivery.controls;}
  
  addRow() {
    // let row = {ProductId: 0, Quantity: 0, ListSerialNumber: [], submited: false};
    // this.ListInventoryLine.push(row);
    let row = {SerialId: 0}
    this.ListRow.push(row)
  }

  onSerialNumberChange(row) {
    const selectedSerial = this.ListSerialComboboxByWH.find(serial => serial.serialId === row.SerialId);
    if (selectedSerial) {
      row.productId = selectedSerial.productId;
      row.price = selectedSerial.price;
      this.updateProductQuantities();
    }
  }

  deleteRow(index:any) {
    // this.ListInventoryLine.splice(index, 1);
    this.ListRow.splice(index, 1);
    this.updateProductQuantities();
  }

  updateProductQuantities() {
    // Reset quantities
    this.ListProductCombobox.forEach(product => product.quantity = 0);

    // Count quantities
    this.ListRow.forEach(row => {
      if (row.productId) {
        const product = this.ListProductCombobox.find(p => p.productId === row.productId);
        if (product) {
          product.quantity++;
        }
      }
    });
  }
  // hasDuplicateProductIds(): boolean {
  //   const productIds: number[] = [];
  //   for (const row of this.ListInventoryLine) {
  //     if (productIds.includes(row.ProductId)) {
  //       return true; // Nếu đã tồn tại ProductId trong mảng, trả về true
  //     }
  //     productIds.push(row.ProductId); // Nếu chưa, thêm ProductId vào mảng
  //   }
  //   return false; // Không có ProductId nào trùng nhau
  // }

  // onSubmit(): void {
  //   this.submited = true;
  //   let hasDuplicateSerial = false;

  //   this.ListInventoryLine.forEach(row => {
  //     if (!row.ProductId || !row.Quantity || row.ListSerialNumber.length != row.Quantity) {
  //       row.submited = true;
  //     } else {
  //       row.submited = false; // Reset submited for valid rows
  //     }

  //     let serialId = new Set(); // Set để lưu trữ các serial number đã xuất hiện trong dòng
  //     row.ListSerialNumber.forEach(e => {
  //       if (!e.SerialId) {
  //         row.submited = true;
  //       } else {
  //         e.submited = false; // Reset submited for valid rows
  //       }

  //       // Nếu serial number đã xuất hiện trước đó trong cùng một dòng, đặt biến cờ là true
  //       if (serialId.has(e.SerialId)) {
  //         hasDuplicateSerial = true;
  //       } else {
  //         serialId.add(e.SerialId); // Thêm serial number vào set
  //       }
  //     });
  //   });

  //   // Kiểm tra xem có ProductId nào trùng nhau không
  //   const hasDuplicateProductId = this.hasDuplicateProductIds();
  //   if (hasDuplicateProductId) {
  //     this.toastr.error('Product must be unique for each row', 'Error');
  //     return;
  //   }

  //   // Nếu có serial number trùng nhau, hiển thị thông báo và không thực hiện hành động tiếp theo
  //   if (hasDuplicateSerial) {
  //     this.toastr.error('Duplicate serial numbers are not allowed in the same row.', 'Error');
  //     return;
  //   }

  //   // Nếu bất kỳ dòng nào không hợp lệ, không thực hiện gửi form và hiển thị thông báo lỗi
  //   if (this.ListInventoryLine.some(row => row.submited)) {
  //     this.toastr.error('Please fill out all fields in each row of the list product table & list serial number and quantity must be > 0', 'Error');
  //     return;
  //   }

  //   if(!this.addDelivery.invalid){
  //     console.log(this.addDelivery.value);
  //     this.SourceLocation = parseInt(this.addDelivery.value.SourceLocation!);
  //     this.WarehouseId = parseInt(this.addDelivery.value.WareHouseId!);
      
  //     console.log(this.SourceLocation, this.WarehouseId, this.ListInventoryLine);
  //     //add
  //     this.inventoryService.insertInventory({
  //       SourceLocation: this.SourceLocation,
  //       WareHouseId: this.WarehouseId,
  //       Type: 2,
  //       ListInventoryLine: this.ListInventoryLine
  //     }).subscribe(res => {
  //       if(res.statusCode == 200){
  //         this.submited = false;
  //         this.router.navigate(['/deliveries/index']).then(() => {
  //           this.toastr.success(res.statusMessage, "Success");
  //         });
  //       }
  //       else if(res.statusCode == 400){
  //         this.toastr.warning(res.statusMessage, "Warning");
  //       }
  //       else if(res.statusCode == 500){
  //         this.toastr.error(res.statusMessage, "Error");
  //       }
  //     })
  //   }
  // }

  isValid() {
    const serialIds = new Set();
    for (const row of this.ListRow) {
      if (!row.SerialId || !row.productId) {
        this.toastr.warning('Serial Number and Product cannot be empty.', 'Warning');
        return false;
      }
      if (serialIds.has(row.SerialId)) {
        this.toastr.warning('Duplicate Serial Number found.', 'Warning');
        return false;
      }
      serialIds.add(row.SerialId);
    }
    return true;
  }

  onSubmit(): void {
    this.submited = true;
    if (!this.isValid()) {
      return;
    }

    this.ListInventoryLine = this.ListProductCombobox
      .filter(product => product.quantity > 0)
      .map(product => ({
        ProductId: product.productId,
        Quantity: product.quantity,
        ListSerialNumber: this.ListRow
        .filter(row => row.productId === product.productId)
        .map(row => ({ SerialId: row.SerialId }))
      }));

    // this.ListInventoryLine = this.ListProductCombobox
    // .filter(product => product.quantity > 0)
    // .map(product => {
    //   const serialIds = [];
    //   this.ListRow
    //     .filter(row => row.productId === product.productId)
    //     .forEach(row => {
    //       serialIds["SerialId"] = row.SerialId; // Hoặc giá trị nào khác mà bạn muốn
    //     });
    //   return {
    //     ProductId: product.productId,
    //     Quantity: product.quantity,
    //     ListSerialNumber: serialIds
    //   };
    // });

    if(!this.addDelivery.invalid){
      console.log(this.addDelivery.value);
      this.SourceLocation = parseInt(this.addDelivery.value.SourceLocation!);
      this.WarehouseId = parseInt(this.addDelivery.value.WareHouseId!);
      
      console.log(this.SourceLocation, this.WarehouseId, this.ListRow, this.ListInventoryLine);
      //add
      this.inventoryService.insertInventory({
        SourceLocation: this.SourceLocation,
        WareHouseId: this.WarehouseId,
        Type: 2,
        ListInventoryLine: this.ListInventoryLine
      }).subscribe(res => {
        if(res.statusCode == 200){
          this.submited = false;
          this.router.navigate(['/deliveries/index']).then(() => {
            this.toastr.success(res.statusMessage, "Success");
          });
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

  // Popup Modal
  OpenModal(row: any) {
    this.submited = true;
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }

    if(!this.addDelivery.invalid){
      this.WarehouseId = parseInt(this.addDelivery.value.WareHouseId!);
      this.serialService.getListSerialCombobox({ProductId: row.ProductId, WareHouseId: this.WarehouseId}).subscribe(res => {
        this.ListSerialCombobox = res;
        this.submited = false;
      })
      this.ListSerial = row.ListSerialNumber;
      if (this.ListSerial.length < row.Quantity) {
        for (let i = this.ListSerial.length; i < row.Quantity; i++) {
          let row = {SerialId: '', submited: false};
          this.ListSerial.push(row);
        }
      } else if (this.ListSerial.length > row.Quantity) {
        this.ListSerial = this.ListSerial.slice(0, row.Quantity);
      }
      
      console.log(this.ListSerial)
    }
    else {
      this.toastr.warning("Please select Warehouse and Product!", "Warning")
    }
  }

  CloseModal() {
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "none";
    }
  }

  scannedBarcode: string = '';
  
  
  openScannerModal() {
    const scannerModal = document.getElementById('scannerModal');
    if (scannerModal) {
      scannerModal.classList.add('show'); // Hiển thị modal
      scannerModal.style.display = 'block';
    }
  }

  closeScannerModal() {
    const scannerModal = document.getElementById('scannerModal');
    if (scannerModal) {
      scannerModal.classList.remove('show'); // Ẩn modal 
      scannerModal.style.display = 'none';
    }
  }

  onBarcodeScanned(result: any) {
    this.scannedBarcode = result?.codeResult?.code ?? ''; // Lấy giá trị barcode từ kết quả quét
    console.log('Scanned Barcode:', this.scannedBarcode); // Kiểm tra xem barcode đã được lấy đúng chưa
    this.closeScannerModal();
  }
}
