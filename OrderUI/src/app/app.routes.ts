import { Routes } from '@angular/router';
import { OrderList } from './components/order-list/order-list';
import { OrderCreate } from './components/order-create/order-create';

export const routes: Routes = [
  {
    path: 'orders',
    component: OrderList,
    title: 'Order Management' 
  },
  { 
    path: 'orders/create',
    component: OrderCreate ,
    title: 'Order Creation'
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