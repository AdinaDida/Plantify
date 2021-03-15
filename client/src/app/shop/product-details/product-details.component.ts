import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/models/product';
import { IProductReview } from 'src/app/models/productReview';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;
  quantity = 1;
  reviews: IProductReview[];
  reviewForm : FormGroup;
  review: IProductReview;

  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute,
              private bcService: BreadcrumbService, private basketService: BasketService,private fb: FormBuilder,) {
    this.bcService.set('@productDetails', ' ')
  }

  ngOnInit(): void {
    this.loadProduct();
    this.loadReviews();
    this.createReviewForm();
  }

  createReviewForm() {
    this.reviewForm = this.fb.group({
        username: [null, Validators.required],
        description: [null, Validators.required],
        title: [null, Validators.required],
    })
  }

  createReview()
  {
    this.review = {
      rating:5,
      productId :+this.activatedRoute.snapshot.paramMap.get('id'),
      username : this.reviewForm.get('username').value,
      description : this.reviewForm.get('description').value,
      title : this.reviewForm.get('title').value,
     }
     this.shopService.createReview(this.review).toPromise();
     this.loadReviews();
     this.reviewForm.reset();
  }

  onCancel()
  {
    this.reviewForm.reset();
    this.loadReviews();
  }

  loadProduct() {
    this.shopService.getProduct(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(product => {
      this.product = product;
      this.bcService.set('@productDetails', product.name);
    }, error => {
      console.log(error);
    });
    
  }
  loadReviews()
  {
    console.log("loading reviews");
    this.shopService.getProductReviews(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(reviews => {
      this.reviews = reviews;
    }, error => {
      console.log(error);
    });
  }

  addItemToBasket() {
    this.basketService.addItemToBasket(this.product, this.quantity);
  }

  incrementQuantity() {
    this.quantity++;
  }

  decrementQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }


}
