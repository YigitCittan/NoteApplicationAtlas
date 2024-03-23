import { Component} from '@angular/core';
import { User } from '../../../../models/user';
import { UserService } from '../../../../services/userService';

@Component({
  selector: 'app-change-email-modal',
  templateUrl: './change-email-modal.component.html',
  styleUrls: ['./change-email-modal.component.css']
  
})


export class ChangeEmailModalComponent {
  userDto = new User();
  
 constructor(private userService: UserService){}
  
 onChangeEmail(userDto: User) {
  const token = this.userService.getToken();
  if (token) {
    this.userService.changeEmail(userDto.email,token)
      .subscribe(() => {
        console.log('Email başarıyla güncellendi.');
        const modal = document.getElementById('changeEmailModal');
        if (modal) {
          modal.classList.remove('show');
          modal.setAttribute('aria-hidden', 'true');
          document.body.classList.remove('modal-open');
          const modalBackdrop = document.getElementsByClassName('modal-backdrop')[0];
          if (modalBackdrop) {
            modalBackdrop.remove();
          }
        }
        this.logout();
      }, error => {
        console.error('Email güncellenirken bir hata oluştu:', error);
      });
  } else {
    console.error('Yetkilendirme hatası: Oturum açılmamış veya token bulunamadı.');
  }
}
logout(): void {
  document.cookie = 'jwtToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
  
  window.location.reload();
}



}
