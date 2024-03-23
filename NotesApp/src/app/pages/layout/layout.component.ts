import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RoleGuard } from '../../services/roleguard';
import { JwtValidationService } from '../../services/JwtValidationService';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent {
  

  constructor(private router: Router, private activatedRoute: ActivatedRoute,private roleguard: RoleGuard, private  jwtValidationService: JwtValidationService) {}
  ngOnInit(): void {
    this.jwtValidationService.validateTokenPeriodically().subscribe(
      (isValid) => {
        if(!isValid)
        {
          this.logout();
        }
      },
      (error) => {
        console.error('Token doğrulama hatası:', error);

      }
    );
  }

  
  isHomeOrProfilePage(): boolean {
    return this.router.url.includes('/home') || this.router.url.includes('/profile') || this.router.url.includes('/admin');
  }

  isHomePage(): boolean {
    return this.router.url.includes('/home');
  }

  isProfilePage(): boolean {
    return this.router.url.includes('/profile');
  }
  isAdminPage(): boolean {
    return this.router.url.includes('/admin');
  }
  isAdmin(): boolean {
    return this.roleguard.isAdmin();
    
  }
  showButton(): boolean {
    return this.roleguard.isAdmin()  
}
logout(): void {
  document.cookie = 'jwtToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
  
  window.location.reload();
}
}
