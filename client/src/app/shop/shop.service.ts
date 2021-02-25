import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagination } from '../models/pagination';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = "https://localhost:44376/api/"
  constructor(private http: HttpClient) { }
  getProducts()
  {
    return this.http.get<IPagination>(this.baseUrl + 'products?pageSize=50');
  }
}
