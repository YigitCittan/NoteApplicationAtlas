import { Component } from '@angular/core';
import { User } from '../../../../models/user';
import { UserService } from '../../../../services/userService';

@Component({
  selector: 'app-change-username-modal',
  templateUrl: './change-username-modal.component.html',
  styleUrls: ['./change-username-modal.component.css']
})
export class ChangeUsernameModalComponent {
  userDto = new User();
  
 constructor(private userService: UserService){}
  
 onChangeUsername(userDto: User) {
  const token = this.userService.getToken();
  if (token) {
    this.userService.changeUsername(userDto.userName,token)
      .subscribe(() => {
        alert('Kullanıcı Adı başarıyla güncellendi.');
        const modal = document.getElementById('changeUsernameModal');
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
      }, error => {
        alert('Kullanıcı adı güncellenirken bir hata oluştu:', );
      });
  } else {
    console.error('Yetkilendirme hatası: Oturum açılmamış veya token bulunamadı.');
  }
}
}
