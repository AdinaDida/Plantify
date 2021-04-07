import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { AdminOrdersComponent } from './admin-orders/admin-orders.component';
import { AdminProductsComponent } from './admin-products/admin-products.component';
import { AdminProductFormComponent } from './admin-product-form/admin-product-form.component';

const routes:Routes = 
[
  {path:'', component:AdminComponent},
  {path: 'orders', component: AdminOrdersComponent, data: {breadcrumb: {alias: 'Orders'}}},
  {path: 'products', component: AdminProductsComponent, data: {breadcrumb: {alias: 'Products'}}},
  {path: 'add-product', component: AdminProductFormComponent, data: {breadcrumb: {alias: 'Add Product'}}}
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports:[RouterModule]
})
export class AdminRoutingModule { }
