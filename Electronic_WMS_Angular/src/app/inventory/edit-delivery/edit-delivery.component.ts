import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import * as feather from 'feather-icons';
import { ToastrService } from 'ngx-toastr';
import { InventoryAPIService } from '../InventoryAPIService';
import { UserAPIService } from 'src/app/users/UserAPIService';
import { WareHouseAPIService } from 'src/app/warehouse/WareHouseAPIService';
import { ProductAPIService } from 'src/app/product/ProductAPIService';
import { SerialNumberAPIService } from 'src/app/serial-number/SerialNumberAPIService';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-edit-delivery',
  templateUrl: './edit-delivery.component.html',
  styleUrls: ['./edit-delivery.component.css']
})
export class EditDeliveryComponent {
  public submited: boolean = false;
  public InventoryId!: number;
  public SourceLocation!: number;
  public WarehouseId!: number;

  // table edit
  public ListInventoryLine: any[] = []; 
  public Quantity!: number;
  public ProductId!: number;

  public ListSerial: any[] = []; 
  public SerialId!: string;

  // Combobox
  public ListShopCombobox: any[] = [];
  public ListWHCombobox: any[] = [];
  public ListSerialCombobox: any[] = [];
  public ListProductCombobox: any[] = [];

  editDelivery = this.fb.group({
    InventoryId: ['', Validators.required],
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
    this.getReceiptDetail();
    this.getListWHCombobox();
    this.getListProductCombobox();
    this.getListUserCombobox();
  }

  getReceiptDetail(): void {
    this.activatedRoute.paramMap.subscribe(query => {
      console.log(query.get("deliveryId"));   // lay id
      let id = query.get("deliveryId");
      if (id != null) {
        let receiptId = parseInt(id);
        this.inventoryService.getInventory(receiptId).subscribe(res => {
          // this.rows = res.listProductFeature;
          res.listInventoryLine.forEach(e => {
            const ListSeri: any[] = [];
            e.listSerialNumber.forEach(ele => {
              const r = {
                SerialId: ele.serialId,
                submited: false,
                shouldDisableCombo: true,
              }
              ListSeri.push(r)
            })
            const row = {
              InventoryLineId: e.inventoryLineId,
              ProductId: e.productId,
              Quantity: e.quantity,
              ListSerialNumber: ListSeri,
              submited: false,
              shouldDisableCombo: true,
            }
            this.ListInventoryLine.push(row)
          });
          this.editDelivery = this.fb.group({
            InventoryId: [res.inventoryId, Validators.required],
            SourceLocation: [res.sourceLocation, Validators.required],
            WareHouseId: [res.wareHouseId, Validators.required]
          });
        });
      }
      else if (id == null) {
        this.toastr.warning("Delivery not found!", "Warning")
      }
    })
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

  getListProductCombobox(): void {
    this.productService.getListProductCombobox().subscribe(res => {
      this.ListProductCombobox = res;
    })
  }

  get f() {return this.editDelivery.controls;}
  
  addRow() {
    let row = {InventoryLineId: 0, ProductId: 0, Quantity: 0, ListSerialNumber: [], submited: false, shouldDisableCombo: false};
    this.ListInventoryLine.push(row);
  }

  deleteRow(index:any) {
    this.ListInventoryLine.splice(index, 1);
  }

  hasDuplicateProductIds(): boolean {
    const productIds: number[] = [];
    for (const row of this.ListInventoryLine) {
      if (productIds.includes(row.ProductId)) {
        return true; // Nếu đã tồn tại ProductId trong mảng, trả về true
      }
      productIds.push(row.ProductId); // Nếu chưa, thêm ProductId vào mảng
    }
    return false; // Không có ProductId nào trùng nhau
  }

  onSubmit(): void {
    this.submited = true;
    let hasDuplicateSerial = false;
    const previousQuantities = this.ListInventoryLine.map(row => row.Quantity);

    this.ListInventoryLine.forEach((row, index) => {
      if (!row.ProductId || !row.Quantity || row.ListSerialNumber.length != row.Quantity) {
        row.submited = true;
      } else {
        row.submited = false; // Reset submited for valid rows
      }

      row.ListSerialNumber.forEach(e => {
        if (!e.SerialId) {
          row.submited = true;
        } else {
          e.submited = false; // Reset submited for valid rows
        }
      });

      // Kiểm tra số lượng của các dòng input
      if (row.Quantity < previousQuantities[index]) {
        row.submited = true;
        this.toastr.error('Quantity cannot be decreased', 'Error');
      }

      let serialNumbers = new Set(); // Set để lưu trữ các serial number đã xuất hiện trong dòng

      // Duyệt qua từng serial number trong dòng hiện tại
      row.ListSerialNumber.forEach(serial => {
        // Nếu serial number đã xuất hiện trước đó trong cùng một dòng, đặt biến cờ là true
        if (serialNumbers.has(serial.SerialId)) {
          hasDuplicateSerial = true;
        } else {
          serialNumbers.add(serial.SerialId); // Thêm serial number vào set
        }
      });
    });

    // Kiểm tra xem có ProductId nào trùng nhau không
    const hasDuplicateProductId = this.hasDuplicateProductIds();
    if (hasDuplicateProductId) {
      this.toastr.error('Product Id must be unique for each row', 'Error');
      return;
    }

    // Nếu có serial number trùng nhau, hiển thị thông báo và không thực hiện hành động tiếp theo
    if (hasDuplicateSerial) {
      this.toastr.error('Duplicate serial numbers are not allowed in the same row.', 'Error');
      return;
    }
    // Nếu bất kỳ dòng nào không hợp lệ, không thực hiện gửi form và hiển thị thông báo lỗi
    if (this.ListInventoryLine.some(row => row.submited)) {
      this.toastr.error('Please fill out all fields and quantity must be > 0 and quantity cannot be decreased', 'Error');
      return;
    }

    if(!this.editDelivery.invalid){
      console.log(this.editDelivery.value);
      this.InventoryId = parseInt(this.editDelivery.value.InventoryId!);
      this.SourceLocation = parseInt(this.editDelivery.value.SourceLocation!);
      this.WarehouseId = parseInt(this.editDelivery.value.WareHouseId!);
      
      console.log(this.SourceLocation, this.WarehouseId, this.ListInventoryLine);
      //edit
      this.inventoryService.updateInventory({
        InventoryId: this.InventoryId,
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

  // Popup Modal
  OpenModal(row: any) {
    this.submited = true;
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }
    
    if(!this.editDelivery.invalid){
      this.WarehouseId = parseInt(this.editDelivery.value.WareHouseId!);
      this.serialService.getListSerialCombobox({ProductId: row.ProductId, WareHouseId: this.WarehouseId}).subscribe(res => {
        this.ListSerialCombobox = res;
        this.submited = false;
      })

      this.ListSerial = row.ListSerialNumber;
      for (let i = this.ListSerial.length; i < row.Quantity; i++) {
        let row = {SerialId: 0, submited: false, shouldDisableCombo: false};
        this.ListSerial.push(row);
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
}
