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
  average = 0;

  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute,
              private bcService: BreadcrumbService, private basketService: BasketService,private fb: FormBuilder,) {
    this.bcService.set('@productDetails', ' ')
  }

  ngOnInit(): void {
    this.createReviewForm();
    this.loadProduct();
    this.loadReviews();
    if(this.reviews){
      this.calculateAverage(this.reviews);
    }
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
     this.shopService.createReview(this.review).toPromise().then(reviews => this.reviews = reviews);
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
    
    this.shopService.getProductReviews(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(reviews => {
      this.reviews = reviews;
    }, error => {
      console.log(error);
    });
    console.log(this.reviews);
    console.log("loading reviews");
    

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

  
  calculateAverage(allReviews){
    let allRatings = 0;
    let count = 0;
    for(let r of allReviews){
      allRatings += r.rating;
      count++;
    }
    this.average = allRatings/count;
    console.log("___________________________")
    console.log(this.average);
    console.log("___________________________")
  }

}
