import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ProductAPIService } from '../ProductAPIService';
import { BrandAPIService } from 'src/app/brand/BrandAPIService';
import { CategoryAPIService } from 'src/app/category/CategoryAPIService';
import { FeatureAPIService } from 'src/app/feature/FeatureAPIService';
import { ActivatedRoute, Router } from '@angular/router';
import * as feather from 'feather-icons';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent {
  public submited: boolean = false;
  public ProductId!: number;
  public ProductName!: string;
  public Description!: string;
  public Price!: number;
  public Quantity!: number;
  public Unit!: string;
  public BrandId!: Number;
  public CateId!: number;
  public Image!: string;
  public FileImage!: File;
  public ListProductFeature!: string;

  public ListPF: any[] = [];
  public ProductFeatureId!: number;
  public FeatureId!: number;
  public Value!: String;
  // table edit
  public rows: any[] = []; 
  // Combobox
  public ListBrandCombobox: any[] = [];
  public ListCateCombobox: any[] = [];
  public ListFeatureCombobox: any[] = [];

  editProduct = this.fb.group({
    ProductId: ['', Validators.required],
    ProductName: ['', Validators.required],
    Description: ['', Validators.required],
    Price: ['', Validators.required],
    Quantity: ['', Validators.required],
    Unit: ['', Validators.required],
    BrandId: ['', Validators.required],
    CateId: ['', Validators.required],
  });
  
  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private productService: ProductAPIService,
    private brandService: BrandAPIService,
    private categoryService: CategoryAPIService,
    private featureService: FeatureAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    feather.replace();
    this.getProduct();
    this.getListBrandCombobox();
    this.getListCateCombobox();
    this.getListFeatureCombobox();
  }

  getProduct(): void {
    this.activatedRoute.paramMap.subscribe(query => {
      console.log(query.get("productId"));   // lay id
      let id = query.get("productId");
      if (id != null) {
        let productId = parseInt(id);
        this.productService.getProduct(productId).subscribe(res => {
          // this.rows = res.listProductFeature;
          res.listProductFeature.forEach(e => {
            const row = {
                ProductFeatureId: e.productFeatureId,
                FeatureId: e.featureId,
                Value: e.value,
                submited: false
              }
            this.rows.push(row)
          });
          this.Image = res.image;
          this.editProduct = this.fb.group({
            ProductId: [res.productId, Validators.required],
            ProductName: [res.productName, Validators.required],
            Description: [res.description, Validators.required],
            Price: [res.price, Validators.required],
            Quantity: [res.quantity, Validators.required],
            Unit: [res.unit, Validators.required],
            BrandId: [res.brandId, Validators.required],
            CateId: [res.cateId, Validators.required],
          });
        });
      }
      else if (id == null) {
        this.toastr.warning("Product not found!", "Warning")
      }
    })
  }

  getListFeatureCombobox(): void {
    this.featureService.getFeatureCombobox().subscribe(res => {
      this.ListFeatureCombobox = res;
    })
  }

  getListBrandCombobox(): void {
    this.brandService.getBrandCombobox().subscribe(res => {
      this.ListBrandCombobox = res;
    })
  }

  getListCateCombobox(): void {
    this.categoryService.getCategoryCombobox().subscribe(res => {
      this.ListCateCombobox = res;
    })
  }

  get f() {return this.editProduct.controls;}
  
  onChange(event: any) {
    this.FileImage = event.target.files[0];
  }

  // addRow() {
  //   let row = { ProductFeatureId: 0,FeatureId: 0, Value: "", submited: false};
  //   this.rows.push(row);
  //   console.log(this.rows)
  // }

  // deleteRow(index:any) {
  //   this.rows.splice(index, 1);
  //   console.log(this.rows)
  // }

  onSubmit(): void {
    this.submited = true;

    this.rows.forEach(row => {
      if (!row.ProductFeatureId || !row.FeatureId || !row.Value) {
        row.submited = true;
      } else {
        row.submited = false; // Reset submited for valid rows
      }
    });
    // Nếu bất kỳ dòng nào không hợp lệ, không thực hiện gửi form và hiển thị thông báo lỗi
    if (this.rows.some(row => row.submited)) {
      this.toastr.error('Please fill out all fields in each row of the product specification table', 'Error');
      return;
    }

    console.log(this.editProduct.invalid)
    if(!this.editProduct.invalid){
      console.log(this.editProduct.value);
      this.ProductId = parseInt(this.editProduct.value.ProductId!);
      this.ProductName= this.editProduct.value.ProductName!.trim();
      this.Description = this.editProduct.value.Description!.trim();
      this.Price = parseInt(this.editProduct.value.Price!);
      this.Quantity = parseInt(this.editProduct.value.Quantity!);
      this.Unit = this.editProduct.value.Unit!.trim();
      this.BrandId = parseInt(this.editProduct.value.BrandId!);
      this.CateId = parseInt(this.editProduct.value.CateId!);
      this.ListProductFeature = JSON.stringify(this.rows);

      var formEdit = new FormData();
      formEdit.append("ProductId", this.ProductId.toString());
      formEdit.append("ProductName", this.ProductName);
      formEdit.append("Description", this.Description);
      formEdit.append("Price", this.Price.toString());
      formEdit.append("Quantity", this.Quantity.toString());
      formEdit.append("Unit", this.Unit);
      formEdit.append("BrandId", this.BrandId.toString());
      formEdit.append("CateId", this.CateId.toString());
      formEdit.append("FileImage", this.FileImage);
      formEdit.append("ListProductFeature", this.ListProductFeature);

      console.log(formEdit)
      
      //update
      this.productService.updateProduct(formEdit).subscribe(res => {
        if(res.statusCode == 200){
          this.submited = false;
          this.router.navigate(['/product/index']).then(() => {
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
}
