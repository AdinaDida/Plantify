import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IOrder } from '../models/order';
import { IProduct } from '../models/product';


@Injectable({
  providedIn: 'root'
})
export class AdminService {
  createProduct(product) {
    return this.http.post<IProduct>(this.baseUrl + 'admin/product', product);
  }
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getOrders(){
    return this.http.get<IOrder[]>(this.baseUrl + 'admin/orders');
  }
  getProducts(){
    return this.http.get<IProduct[]>(this.baseUrl + 'admin/products');
  }
  changeOrderStatus(id, status){
    return this.http.patch<IOrder>(this.baseUrl + 'admin/order/' + id + "/" + status, {}, {});

  }
  deleteProduct(id){
    return this.http.delete<IProduct>(this.baseUrl + 'admin/delete-product/' + id);
  }

  getFinishedOrders(){
    return this.http.get<IOrder[]>(this.baseUrl + 'admin/orders/finished');
  }

  getOrderDetails(id) {
    return this.http.get<IOrder>(this.baseUrl + 'admin/order/' + id);
  }

}


