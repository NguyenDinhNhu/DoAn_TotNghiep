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

const routes: Routes = [
  { path: '', component: DashboardComponent},
  { path: 'brand/index', component: ListBrandComponent},
  { path: 'category/index', component: CategoryComponent},
  { path: 'feature/index', component: FeatureComponent},
  { path: 'roles/index', component: RolesComponent},
  { path: 'warehouse/index', component: WarehouseComponent},
  { path: 'user/index', component: ListUserComponent},
  { path: 'user/add', component: AddUserComponent},
  { path: 'user/edit/:userId', component: EditUserComponent},
  { path: 'product/index', component: ListProductComponent},
  { path: 'product/add', component: AddProductComponent},
  { path: 'product/edit/:productId', component: EditProductComponent},
  { path: 'product-feature/index/:productId', component: ProductFeatureComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
