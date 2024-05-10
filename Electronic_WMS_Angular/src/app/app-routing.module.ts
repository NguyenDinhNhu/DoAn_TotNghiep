import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ListBrandComponent } from './brand/list-brand/list-brand.component';
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
import { EditReceiptComponent } from './inventory/edit-receipt/edit-receipt.component';
import { SerialNumberComponent } from './serial-number/serial-number.component';
import { MoveHistoryComponent } from './report/move-history/move-history.component';
import { EditDeliveryComponent } from './inventory/edit-delivery/edit-delivery.component';
import { StockComponent } from './report/stock/stock.component';
import { InventoryDetailComponent } from './inventory/inventory-detail/inventory-detail.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './login/AuthGuard';
import { UserDetailComponent } from './users/user-detail/user-detail.component';
import { ProductDetailComponent } from './product/product-detail/product-detail.component';

const routes: Routes = [
  { path: '', component: DashboardComponent, canActivate: [AuthGuard]  },
  { path: 'brand/index', component: ListBrandComponent, canActivate: [AuthGuard] },
  { path: 'category/index', component: CategoryComponent, canActivate: [AuthGuard] },
  { path: 'feature/index', component: FeatureComponent, canActivate: [AuthGuard] },
  { path: 'roles/index', component: RolesComponent, canActivate: [AuthGuard] },
  { path: 'warehouse/index', component: WarehouseComponent, canActivate: [AuthGuard] },
  { path: 'user/index', component: ListUserComponent, canActivate: [AuthGuard] },
  { path: 'user/add', component: AddUserComponent, canActivate: [AuthGuard] },
  { path: 'user/edit/:userId', component: EditUserComponent, canActivate: [AuthGuard] },
  { path: 'user/detail/:userId', component: UserDetailComponent, canActivate: [AuthGuard] },
  { path: 'product/index', component: ListProductComponent, canActivate: [AuthGuard] },
  { path: 'product/add', component: AddProductComponent, canActivate: [AuthGuard] },
  { path: 'product/edit/:productId', component: EditProductComponent, canActivate: [AuthGuard] },
  { path: 'product/detail/:productId', component: ProductDetailComponent, canActivate: [AuthGuard] },
  { path: 'product-feature/index/:productId', component: ProductFeatureComponent, canActivate: [AuthGuard] },
  { path: 'receipts/index', component: ListReceiptsComponent, canActivate: [AuthGuard] },
  { path: 'deliveries/index', component: ListDeliveriesComponent, canActivate: [AuthGuard] },
  { path: 'receipts/add', component: AddReceiptComponent, canActivate: [AuthGuard] },
  { path: 'deliveries/add', component: AddDeliveryComponent, canActivate: [AuthGuard] },
  { path: 'receipts/edit/:receiptId', component: EditReceiptComponent, canActivate: [AuthGuard] },
  { path: 'deliveries/edit/:deliveryId', component: EditDeliveryComponent, canActivate: [AuthGuard] },
  { path: 'inventory/detail/:inventoryId', component: InventoryDetailComponent, canActivate: [AuthGuard] },
  { path: 'serial-number/index/:productId', component: SerialNumberComponent, canActivate: [AuthGuard] },
  { path: 'report/move-history', component: MoveHistoryComponent, canActivate: [AuthGuard] },
  { path: 'report/stock', component: StockComponent, canActivate: [AuthGuard] },

  { path: 'login', component: LoginComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
