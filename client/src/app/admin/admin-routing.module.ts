import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { AdminOrdersComponent } from './admin-orders/admin-orders.component';
import { AdminProductsComponent } from './admin-products/admin-products.component';
import { AdminProductFormComponent } from './admin-product-form/admin-product-form.component';
import { AdminHistoryComponent } from './admin-history/admin-history.component';
import { AdminOrderDetailsComponent } from './admin-order-details/admin-order-details.component';

const routes:Routes = 
[
  {path:'', component:AdminComponent},
  {path: 'orders', component: AdminOrdersComponent, data: {breadcrumb: 'Orders'}},
  {path: 'products', component: AdminProductsComponent, data: {breadcrumb: 'Products'}},
  {path: 'add-product', component: AdminProductFormComponent, data: {breadcrumb: 'Add Product'}},
  {path: 'orders-history', component: AdminHistoryComponent, data: {breadcrumb: 'Orders History'}},
  {path: 'order/:id', component: AdminOrderDetailsComponent, data: {breadcrumb: {alias: 'OrderDetails'}}}
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
