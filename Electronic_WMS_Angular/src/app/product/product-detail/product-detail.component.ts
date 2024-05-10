import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import * as feather from 'feather-icons';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { ProductAPIService } from '../ProductAPIService';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent {
  public productDetail!: any;
  public productPrice: number = 0;

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private productService: ProductAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    feather.replace();
    this.getDetail();
  }


  formatVND(productPrice: number): string {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(productPrice);
  }
  
  getDetail(): void {
    this.activatedRoute.paramMap.subscribe(query => {
      console.log(query.get("productId"));   // lay id
      let id = query.get("productId");
      if (id != null) {
        let productId = parseInt(id);
        this.productService.getProduct(productId).subscribe(res => {
          this.productDetail = res;
        });
      }
      else if (id == null) {
        this.toastr.warning("Receipt not found!", "Warning")
      }
    })
  }
}
