import { Component, OnInit } from '@angular/core';
import { IProduct } from 'src/app/models/product';
import { IBrand } from 'src/app/models/productBrand';
import { IType } from 'src/app/models/productType';
import { ShopService } from 'src/app/shop/shop.service';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-admin-products',
  templateUrl: './admin-products.component.html',
  styleUrls: ['./admin-products.component.scss']
})
export class AdminProductsComponent implements OnInit {

  products:IProduct[]
  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders(){
    this.adminService.getProducts().subscribe(products => {
      this.products = products;
    }, error => {
      console.log(error);
    });
  }

  deleteProduct(id){
    this.adminService.deleteProduct(id).toPromise().then(x => this.getOrders());
  }

  

}
