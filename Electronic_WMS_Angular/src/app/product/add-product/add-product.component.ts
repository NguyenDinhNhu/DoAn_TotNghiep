import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
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
    CateId: ['', Validators.required]
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
    let row = {FeatureId: "", Value: ""};
    this.rows.push(row);
  }

  deleteRow(index:any) {
    this.rows.splice(index, 1);
  }


  onSubmit(): void {
    this.submited = true;
    
    console.log(this.addProduct.invalid)
    if(!this.addProduct.invalid){
      console.log(this.addProduct.value);
      this.ProductName= this.addProduct.value.ProductName!.trim();
      this.Description = this.addProduct.value.Description!.trim();
      this.Price = parseInt(this.addProduct.value.Price!);
      this.Unit = this.addProduct.value.Unit!.trim();
      this.BrandId = parseInt(this.addProduct.value.BrandId!);
      this.CateId = parseInt(this.addProduct.value.CateId!);
      
      var formAdd = new FormData();
      formAdd.append("FullName", this.ProductName);
      formAdd.append("Address", this.Description);
      formAdd.append("PassWord", this.Price.toString());
      formAdd.append("UserName", this.Unit);
      formAdd.append("RoleId", this.BrandId.toString());
      formAdd.append("RoleId", this.CateId.toString());
      formAdd.append("FileImage", this.FileImage);

      console.log(formAdd)
      //thêm mới
      // this.productService.insertProduct(formAdd).subscribe(res => {
      //   if(res.statusCode == 200){
      //     this.submited = false;
      //     this.router.navigate(['/user/index']).then(() => {
      //       this.toastr.success(res.statusMessage, "Success");
      //     });
      //   }
      //   else if(res.statusCode == 400){
      //     this.toastr.warning(res.statusMessage, "Warning");
      //   }
      //   else if(res.statusCode == 500){
      //     this.toastr.error(res.statusMessage, "Error");
      //   }
      // })
    }
  }
}
