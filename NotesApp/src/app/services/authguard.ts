import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { RoleGuard } from './roleguard';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private roleGuard: RoleGuard) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    return this.roleGuard.canActivate(route);
  }
}
