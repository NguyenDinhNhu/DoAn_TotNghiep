import { Component } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as feather from 'feather-icons';
import { ToastrService } from 'ngx-toastr';
import { ProductAPIService } from '../ProductAPIService';
import { BrandAPIService } from 'src/app/brand/BrandAPIService';
import { CategoryAPIService } from 'src/app/category/CategoryAPIService';
import { FeatureAPIService } from 'src/app/feature/FeatureAPIService';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent {
  public submited: boolean = false;
  public ProductName!: string;
  public Description!: string;
  public Price!: number;
  public Unit!: string;
  public BrandId!: Number;
  public CateId!: number;
  public FileImage!: File;
  public ListProductFeature!: string;

  public ListPF: any[] = [];
  public FeatureId!: number;
  public Value!: String;
  // table edit
  public rows: any[] = []; 
  // Combobox
  public ListBrandCombobox: any[] = [];
  public ListCateCombobox: any[] = [];
  public ListFeatureCombobox: any[] = [];

  addProduct = this.fb.group({
    ProductName: ['', Validators.required],
    Description: ['', Validators.required],
    Price: ['', Validators.required],
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
    this.getListBrandCombobox();
    this.getListCateCombobox();
    this.getListFeatureCombobox();
    // this.rows = [{
    //   FeatureId:'',
    //   Value:''
    // }]
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

  get f() {return this.addProduct.controls;}
  
  onChange(event: any) {
    this.FileImage = event.target.files[0];
  }

  addRow() {
    let row = {FeatureId: 0, Value: "", submited: false};
    this.rows.push(row);
    console.log(this.rows)
  }

  deleteRow(index:any) {
    this.rows.splice(index, 1);
    console.log(this.rows)
  }


  onSubmit(): void {
    this.submited = true;

    this.rows.forEach(row => {
      if (!row.FeatureId || !row.Value) {
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

    console.log(this.addProduct.invalid)
    if(!this.addProduct.invalid){
      console.log(this.addProduct.value);
      this.ProductName= this.addProduct.value.ProductName!.trim();
      this.Description = this.addProduct.value.Description!.trim();
      this.Price = parseInt(this.addProduct.value.Price!);
      this.Unit = this.addProduct.value.Unit!.trim();
      this.BrandId = parseInt(this.addProduct.value.BrandId!);
      this.CateId = parseInt(this.addProduct.value.CateId!);
      this.ListProductFeature = JSON.stringify(this.rows);

      var formAdd = new FormData();
      formAdd.append("ProductName", this.ProductName);
      formAdd.append("Description", this.Description);
      formAdd.append("Price", this.Price.toString());
      formAdd.append("Unit", this.Unit);
      formAdd.append("BrandId", this.BrandId.toString());
      formAdd.append("CateId", this.CateId.toString());
      formAdd.append("FileImage", this.FileImage);
      formAdd.append("ListProductFeature", this.ListProductFeature);

      console.log(formAdd)
      
      //add
      this.productService.insertProduct(formAdd).subscribe(res => {
        if(res.statusCode == 200){
          this.submited = false;
          this.router.navigate(['/product/index']).then(() => {
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
}
