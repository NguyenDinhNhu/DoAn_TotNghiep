import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ComboBoxModule } from '@syncfusion/ej2-angular-dropdowns';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
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
import { AddReceiptComponent } from './inventory/add-receipt/add-receipt.component';
import { AddDeliveryComponent } from './inventory/add-delivery/add-delivery.component';
import { SerialNumberComponent } from './serial-number/serial-number.component';
import { EditReceiptComponent } from './inventory/edit-receipt/edit-receipt.component';
import { MoveHistoryComponent } from './report/move-history/move-history.component';
import { EditDeliveryComponent } from './inventory/edit-delivery/edit-delivery.component';
import { StockComponent } from './report/stock/stock.component';
import { InventoryDetailComponent } from './inventory/inventory-detail/inventory-detail.component';
import { LoginComponent } from './login/login.component';
import { UserDetailComponent } from './users/user-detail/user-detail.component';
import { ProductDetailComponent } from './product/product-detail/product-detail.component';
import {MatMenuModule} from '@angular/material/menu';
import {MatButtonModule} from '@angular/material/button';
import { AuthAPIService } from './login/AuthAPIService';
import { AuthInterceptor } from './login/auth.interceptor';

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
    AddReceiptComponent,
    AddDeliveryComponent,
    SerialNumberComponent,
    EditReceiptComponent,
    MoveHistoryComponent,
    EditDeliveryComponent,
    StockComponent,
    InventoryDetailComponent,
    LoginComponent,
    UserDetailComponent,
    ProductDetailComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule,
    AppRoutingModule, FormsModule, ReactiveFormsModule, ComboBoxModule, BrowserAnimationsModule,
    HttpClientModule,
    CommonModule,IgxComboModule, IgxSimpleComboModule,
    ToastrModule.forRoot({timeOut: 3000, positionClass: 'toast-top-right',}),
    MatMenuModule,
    MatButtonModule
  ],
  providers: [
    AuthAPIService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
