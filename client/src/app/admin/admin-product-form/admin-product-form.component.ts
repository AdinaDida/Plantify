import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IProduct } from 'src/app/models/product';
import { IBrand } from 'src/app/models/productBrand';
import { IType } from 'src/app/models/productType';
import { ShopService } from 'src/app/shop/shop.service';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-admin-product-form',
  templateUrl: './admin-product-form.component.html',
  styleUrls: ['./admin-product-form.component.scss']
})
export class AdminProductFormComponent implements OnInit {
  productForm : FormGroup;
  product;
  productBrands: IBrand[]
  productTypes: IType[]
  selectBrand="Select Brand"
  selectType="Select Type"
  constructor(private fb: FormBuilder, private adminService:AdminService, private shopService: ShopService) { }

  ngOnInit(): void {
    this.createProductForm();
    this.getProductBrands();
    this.getProductTypes();
  }

  createProductForm() {
    this.productForm = this.fb.group({
      name: [null, Validators.required],
      description: [null, Validators.required],
      price: [null, Validators.required],
      pictureUrl: [null, Validators.required],
      productTypeId: [null, Validators.required],
      productBrandId: [null, Validators.required],

    })
  }

  onCancel(){
    this.productForm.reset();
  }

  createProduct(){
    this.product = {
      name : this.productForm.get('name').value,
      description : this.productForm.get('description').value,
      pictureUrl : "images/products/flowers/" + this.productForm.get('pictureUrl').value.match(/[^\\/]*$/)[0],
      price : +this.productForm.get('price').value,
      productTypeId : +this.productForm.get('productTypeId').value,
      productBrandId : +this.productForm.get('productBrandId').value,
    }
    this.adminService.createProduct(this.product).toPromise();
    this.productForm.reset();
    }

    getProductBrands(){
      this.shopService.getBrands().subscribe(brands => this.productBrands = brands);
    }
  
    getProductTypes(){
      this.shopService.getTypes().subscribe(types => this.productTypes = types);
    }

}
