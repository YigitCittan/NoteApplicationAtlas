// change-password-modal.component.ts

import { Component } from '@angular/core';
import { User } from '../../../../models/user';
import { UserService } from '../../../../services/userService';

@Component({
  selector: 'app-change-password-modal',
  templateUrl: './change-password-modal.component.html',
  styleUrls: ['./change-password-modal.component.css']
})
export class ChangePasswordModalComponent {
  userDto = new User();

 constructor(private userService: UserService){}


  onChangePassword(userDto: User) {
    const token = this.userService.getToken();
    if (token) {
      this.userService.changePassword(userDto.recentPassword,userDto.password,token)
        .subscribe(() => {
          alert('Şifre başarıyla güncellendi.');
          this.logout();
          const modal = document.getElementById('changePasswordModal');
          if (modal) {
            modal.classList.remove('show');
            modal.setAttribute('aria-hidden', 'true');
            document.body.classList.remove('modal-open');
            const modalBackdrop = document.getElementsByClassName('modal-backdrop')[0];
            if (modalBackdrop) {
              modalBackdrop.remove();
            }
          }
          window.location.reload();
        }, (error: any) => {
          alert('Kullanımda olan şifreniz veya yeni şifreniz hatalı',);
        });
    } else {
      alert('Yetkilendirme hatası: Oturum açılmamış veya token bulunamadı.');
    }
  }
  
  logout(): void {
    document.cookie = 'jwtToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
    
    window.location.reload();
  }
  
}
