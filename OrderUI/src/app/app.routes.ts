import { Routes } from '@angular/router';
import { OrderList } from './components/order-list/order-list';

export const routes: Routes = [
  {
    path: 'orders',
    component: OrderList,
    title: 'Order Management' 
  },
  {
    path: '',
    redirectTo: 'orders',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'orders'
  }
];