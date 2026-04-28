import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { OrderService } from '../../services/order';

@Component({
  selector: 'app-order-create',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './order-create.html',
  styleUrls: ['./order-create.scss']
})
export class OrderCreate {
  private readonly fb = inject(FormBuilder);
  private readonly orderService = inject(OrderService);
  private readonly router = inject(Router);

  orderForm = this.fb.group({
    userName: ['', [Validators.required, Validators.minLength(3)]],
    productName: ['', [Validators.required]],
    quantity: [1, [Validators.required, Validators.min(1)]],
    price: [0, [Validators.required, Validators.min(0.01)]]
  });

  onSubmit(): void {
    if (this.orderForm.valid) {
      const formValue = this.orderForm.value;
      
      const newOrder = {
        userName: formValue.userName,
        items: [{
          productName: formValue.productName,
          quantity: formValue.quantity,
          price: formValue.price
        }]
      };

      console.log('[OrderUI] Sending new order:', newOrder);

      this.orderService.createOrder(newOrder).subscribe({
        next: () => {
          console.log('[OrderUI] Order created successfully!');
          this.router.navigate(['/orders']);
        },
        error: (err) => {
          console.error('[OrderUI] CREATE_ORDER_ERROR:', err);
          alert('Failed to create order. Check console for details.');
        }
      });
    }
  }
}