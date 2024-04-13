import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ProductFeatureAPIService } from './ProductFeatureAPIService';
import * as feather from 'feather-icons';
import Swal from 'sweetalert2';
import { FeatureAPIService } from '../feature/FeatureAPIService';
import { ProductAPIService } from '../product/ProductAPIService';

@Component({
  selector: 'app-product-feature',
  templateUrl: './product-feature.component.html',
  styleUrls: ['./product-feature.component.css']
})
export class ProductFeatureComponent {
  public listPF: any[] = [];
  public productFeatureId!: number;
  public featureId!: number;
  public productId!: number;
  public value!: string;
  public submitedAdd: boolean = false;
  public submitedEdit: boolean = false;
  public isDisabled: boolean = true;

  // Form
  addPFForm = this.fb.group({
    FeatureId: ['', Validators.required],
    ProductId: [0, Validators.required],
    Value: ['', Validators.required]
  });

  editPFForm = this.fb.group({
    ProductFeatureId: ['', Validators.required],
    FeatureId: ['', Validators.required],
    ProductId: ['', Validators.required],
    Value: ['', Validators.required]
  });
  
  // combobox
  public listFeatureCombobox: any[] = [];
  public listProductCombobox: any[] = [];

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private pfService: ProductFeatureAPIService,
    private productService: ProductAPIService,
    private featureService: FeatureAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  get f() {return this.addPFForm.controls;}
  get v() {return this.editPFForm.controls;}

  ngOnInit(): void {
    feather.replace();
    this.getAllProductFeature();
    this.getListFeatureCombobox();
    this.getListProductCombobox();
  }

  // Add 
  addProductFeature(): void {
    this.submitedAdd = true;
    
    if(!this.addPFForm.invalid){
      console.log(this.addPFForm.value);
      this.featureId = parseInt(this.addPFForm.value.FeatureId!);
      this.productId = this.addPFForm.value.ProductId!;
      this.value = this.addPFForm.value.Value!.trim();
      //call api add 
      this.pfService.insertProductFeature( {  
        ProductId: this.productId,
        FeatureId: this.featureId,
        Value: this.value }).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.submitedAdd = false;
            this.getAllProductFeature();
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
  editProductFeature(): void {
    this.submitedEdit = true;
    
    if(!this.editPFForm.invalid){
      console.log(this.editPFForm.value);
      this.productFeatureId = parseInt(this.editPFForm.value.ProductFeatureId!);
      this.featureId = parseInt(this.editPFForm.value.FeatureId!);
      this.value = this.editPFForm.value.Value?.trim()!;
      this.productId = parseInt(this.editPFForm.value.ProductId!);
      //call api update 
      this.pfService.updateProductFeature( { 
        ProductFeatureId: this.productFeatureId,
        ProductId: this.productId,
        FeatureId: this.featureId,
        Value: this.value }).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.submitedAdd = false;
            this.getAllProductFeature();
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
  deleteProductFeature(id: number): void {
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
        this.pfService.deleteProductFeature(id).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.getAllProductFeature();
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

  getAllProductFeature(): any {
    this.activatedRoute.paramMap.subscribe(query => {
      console.log(query.get("productId"));   // lay id
      let id = query.get("productId");
      if (id != null) {
        let productId = parseInt(id);
        this.pfService.getListByProductId(productId).subscribe(res => {
          this.listPF = res;
          setTimeout(() => {
            feather.replace();
          }, 10)
        });
      }
      else if (id == null) {
        this.toastr.warning("Product not found!", "Warning")
      }
    })
  }

  getListFeatureCombobox(): void {
    this.featureService.getFeatureCombobox().subscribe(res => {
      this.listFeatureCombobox = res;
    })
  }

  getListProductCombobox(): void {
    this.productService.getListProductCombobox().subscribe(res => {
      this.listProductCombobox = res;
    })
  }

  // Popup Modal 
  OpenModal() {
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }
    
    this.activatedRoute.paramMap.subscribe(query => {
      console.log(query.get("productId"));   // lay id
      var productId = parseInt(query.get("productId")!);
      this.addPFForm = this.fb.group({
        FeatureId: ['', Validators.required],
        ProductId: [productId, Validators.required],
        Value: ['', Validators.required]
      });
      console.log(this.addPFForm.value)
    });
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

    this.pfService.getProductFeature(id).subscribe(res => {
      this.editPFForm = this.fb.group({
        ProductFeatureId: [res.productFeatureId, Validators.required],
        FeatureId: [res.featureId, Validators.required],
        ProductId: [res.productId, Validators.required],
        Value: [res.value, Validators.required]
      });
    })
    console.log(this.editPFForm.value)
  }

  CloseModalEdit() {
    const modelDiv = document.getElementById("editModal");
    if(modelDiv != null) {
      modelDiv.style.display = "none";
    }
  }
}
