import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ComboBoxModule } from '@syncfusion/ej2-angular-dropdowns';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { IgxComboModule, IgxSimpleComboModule } from "igniteui-angular";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ListBrandComponent } from './brand/list-brand/list-brand.component';
import { PaginationComponent } from './pagination/pagination.component';
import { ToastrModule } from 'ngx-toastr';
import { CategoryComponent } from './category/category.component';
import { FeatureComponent } from './feature/feature.component';
import { RolesComponent } from './roles/roles.component';
import { WarehouseComponent } from './warehouse/warehouse.component';
import { ListUserComponent } from './users/list-user/list-user.component';
import { AddUserComponent } from './users/add-user/add-user.component';
import { EditUserComponent } from './users/edit-user/edit-user.component';
import { ListProductComponent } from './product/list-product/list-product.component';
import { AddProductComponent } from './product/add-product/add-product.component';
import { ProductFeatureComponent } from './product-feature/product-feature.component';
import { EditProductComponent } from './product/edit-product/edit-product.component';
import { ListReceiptsComponent } from './inventory/list-receipts/list-receipts.component';
import { ListDeliveriesComponent } from './inventory/list-deliveries/list-deliveries.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    ListBrandComponent,
    PaginationComponent,
    CategoryComponent,
    FeatureComponent,
    RolesComponent,
    WarehouseComponent,
    ListUserComponent,
    AddUserComponent,
    EditUserComponent,
    ListProductComponent,
    AddProductComponent,
    ProductFeatureComponent,
    EditProductComponent,
    ListReceiptsComponent,
    ListDeliveriesComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule,
    AppRoutingModule, FormsModule, ReactiveFormsModule, ComboBoxModule, BrowserAnimationsModule,
    HttpClientModule,
    CommonModule,IgxComboModule, IgxSimpleComboModule,
    ToastrModule.forRoot({timeOut: 3000, positionClass: 'toast-top-right',})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
