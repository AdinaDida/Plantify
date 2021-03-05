import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagination } from '../models/pagination';
import { IBrand } from '../models/productBrand';
import { IType } from '../models/productType';
import {map} from 'rxjs/operators';
import { ShopParams } from '../models/shopParams';
import { IProduct } from '../models/product';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  getProducts(shopParams: ShopParams)
  {
    let params = new HttpParams();
    if (shopParams.brandId !== 0)
    {
      params = params.append('brandId', shopParams.brandId.toString());
    }
    if (shopParams.typeId !== 0)
    {
      params = params.append('typeId', shopParams.typeId.toString());
    }
    if (shopParams.search)
    {
      params = params.append('search', shopParams.search);
    }

    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageIndex', shopParams.pageSize.toString());

    return this.http.get<IPagination>(this.baseUrl + 'products', {observe: 'response', params})
    .pipe(map(response => response.body));
  }

  getProduct(id: number)
  {
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

  getBrands()
  {
    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands');
  }
  getTypes()
  {
    return this.http.get<IType[]>(this.baseUrl + 'products/types');
  }
}
