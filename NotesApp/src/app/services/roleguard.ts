import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const cookies = document.cookie.split(';'); 
    let token = ''; 

    for (const cookie of cookies) {
        const [name, value] = cookie.trim().split('=');
        if (name === 'jwtToken') {
            token = value; 
            break;
        }
    }

    if (token) {
        const decodedToken = this.decodeToken(token);
        const userRoles = decodedToken && decodedToken.role;

        if (userRoles && (userRoles.includes('Admin') || userRoles.includes('User')) ) {
            return true;
        } else {
            this.router.navigateByUrl('/unauthorized');
            return false;
        }
    } else {
        this.router.navigateByUrl('/login');
        return false;
    }
}


isAdmin(): boolean {
    const cookies = document.cookie.split(';');

    let token = ''; 

    for (const cookie of cookies) {
        const [name, value] = cookie.trim().split('=');
        if (name === 'jwtToken') {
            token = value; 
            break;
        }
    }

    if (token) {
        const decodedToken = this.decodeToken(token);
        // if (decodedToken && (decodedToken.role === 'admin') || (decodedToken.role === 'admin') && decodedToken.role.includes('user') ) {
if (decodedToken && (decodedToken.role && decodedToken.role.includes('Admin') && decodedToken.role.includes('User')) || (decodedToken.role === 'Admin')){
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}


private decodeToken(token: string): any {
    try {
        const payload = token.split('.')[1];
        return JSON.parse(atob(payload));
    } catch (error) {
        console.error('Token decode hatası:', error);
        return null;
    }
}

}
