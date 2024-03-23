import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { LayoutComponent } from './pages/layout/layout.component';
import { HomeComponent } from './pages/home/home.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { RoleGuard } from './services/roleguard';
import { AdminComponent } from './pages/admin/admin.component';

const routes: Routes = [
  {
    path:'login',
    component: LoginComponent
  },
  {
    path:'',
    redirectTo:'login',
    pathMatch:'full'
  },
  {
    path:'',
    component: LayoutComponent,
    children:[
      {
        path:'home',
        component: HomeComponent,
        canActivate:[RoleGuard],
        data: { roles: ['user','admin'] }
      },
      {
        path:'profile',
        component: ProfileComponent,
        canActivate:[RoleGuard],
        data: { roles: ['user','admin'] }
      },
      {
        path:'admin',
        component: AdminComponent,
        canActivate:[RoleGuard],
        data: { roles: ['admin'] },
      }
      
    ]
  },
  {
    path:'**',
    component:LoginComponent
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
