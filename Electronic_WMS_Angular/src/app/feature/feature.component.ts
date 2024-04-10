import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { FeatureAPIService } from './FeatureAPIService';
import { ActivatedRoute, Router } from '@angular/router';
import * as feather from 'feather-icons';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-feature',
  templateUrl: './feature.component.html',
  styleUrls: ['./feature.component.css']
})
export class FeatureComponent {
  // Search - Get List 
  public pageSize: number = 10;
  public currentPage: number = 1;
  public textSearch!: string;
  public listFeature: any[] = [];
  public totalItem: number = 0;

  public featureName!: string;
  public featureId!: number;
  // Form add 
  public submitedAdd: boolean = false;

  addFeatureForm = this.fb.group({
    FeatureName: ['', Validators.required]
  });

  // Form edit
  public submitedEdit: boolean = false;

  editFeatureForm = this.fb.group({
    FeatureId: ['', Validators.required],
    FeatureName: ['', Validators.required]
  });

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private featureService: FeatureAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  get f() {return this.addFeatureForm.controls;}
  get v() {return this.editFeatureForm.controls;}

  ngOnInit(): void {
    feather.replace();
    this.getAllFeature();
  }

  // Add 
  addFeature(): void {
    this.submitedAdd = true;
    
    if(!this.addFeatureForm.invalid){
      console.log(this.addFeatureForm.value);
      this.featureName = this.addFeatureForm.value.FeatureName!.trim();
      //call api add 
      this.featureService.insertFeature( {FeatureName: this.featureName}).subscribe(res => {
        if(res.statusCode == 200){
          this.toastr.success(res.statusMessage, "Success");
          this.submitedAdd = false;
          this.getAllFeature();
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
  editFeature(): void {
    this.submitedEdit = true;
    
    if(!this.editFeatureForm.invalid){
      console.log(this.editFeatureForm.value);
      this.featureId = parseInt(this.editFeatureForm.value.FeatureId!);
      this.featureName = this.editFeatureForm.value.FeatureName?.trim()!;
      //call api update 
      this.featureService.updateFeature( { 
        FeatureId: this.featureId,
        FeatureName: this.featureName }).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.submitedAdd = false;
            this.getAllFeature();
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

  // Delete
  deleteFeature(id: number): void {
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
        this.featureService.deleteFeature(id).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.getAllFeature();
            this.CloseModalEdit();
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

  getAllFeature(): any {
    this.featureService.getListFeature({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listFeature = res.listFeature;
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
    this.featureService.getListFeature({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listFeature = res.listFeature;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  onChangePageSize(event: any) {
    this.pageSize = event.target.value;
    this.featureService.getListFeature({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listFeature = res.listFeature;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  search(): void {
    this.featureService.getListFeature({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch.trim()}).subscribe(res => {
      this.listFeature = res.listFeature;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  // Popup Modal Add   
  OpenModal() {
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }
    this.addFeatureForm.reset();
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

    this.featureService.getFeature(id).subscribe(res => {
      this.editFeatureForm = this.fb.group({
        FeatureId: [res.featureId, Validators.required],
        FeatureName: [res.featureName, Validators.required]
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
