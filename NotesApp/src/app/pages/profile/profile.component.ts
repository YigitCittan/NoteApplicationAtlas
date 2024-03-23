import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { UserService } from '../../services/userService';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit{
  showProfile = true;
  profileImage: string | ArrayBuffer | null = 'https://bootdey.com/img/Content/avatar/avatar7.png';
  emailDto = new User();
  usernameDto = new User();
  passwordDto = new User();
  recentPasswordDto = new User();
  email='';
  userName='';
  http: any;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.getUserInfo();
    this.getProfileImage();
  }
  
 
  deleteProfilePhoto(){
    const token = this.userService.getToken();
    if (token) {
        this.userService.deleteProfileImage(token).subscribe(
            () => {
                alert('Fotoğraf başarıyla silindi.');
                this.profileImage = 'https://bootdey.com/img/Content/avatar/avatar7.png';
                
            },
            (error) => {
                console.error('Fotoğraf silinirken bir hata oluştu:', error);
                
            }
        );
        window.location.reload();

    }  
  }
addProfileImage(profileImage: string): void {
    const token = this.userService.getToken();
    if (token) {
    this.userService.addProfileImage(token,profileImage)
      .subscribe(
        () => {
          alert('Profil fotoğrafı başarıyla eklendi.');
        },
        (error) => {
          console.error('Profil fotoğrafı eklenirken hata oluştu:', error);
        }
      );
  }
}

getProfileImage() {
  const token = this.userService.getToken();
  if (token) {
    this.userService.getProfileImage(token).subscribe(
      (profileImageData: string) => { 
          this.profileImage = profileImageData;
      },
      (error) => {
          if (error.status === 404) {
              console.log('Kullanıcı profil fotoğrafı bulunamadı.');
          } else {
              console.error('Profil fotoğrafı alınırken bir hata oluştu:', error);
          }
          if (error.status === 500) {
            console.log('Kullanıcı profil fotoğrafı bulunamadı.');
          }
      }
    );
  }
}

deleteUser() {
  const token = this.userService.getToken();
  if (token) {
    this.userService.lockUser(token)
      .subscribe(
        response => {
          console.log('Kullanıcı kilitleme isteği başarılı:', response);
          this.logout();
        },
        error => {
          console.error('Kullanıcı kilitleme isteği hatası:', error);
        }
      );
  } else {
    console.error('Token bulunamadı!');
  }
}

  getUserInfo(): void {
  const token = this.userService.getToken();
  if (token) {
    this.userService.getUserInfo(token).subscribe(
      (userInfo: User) => {
        if (userInfo) {
          this.userName = userInfo.userName;
          this.email = userInfo.email;

          
        } else {
          console.error('Kullanıcı bilgileri alınamadı.');
        }
      },
      (error) => {
        console.error('Kullanıcı bilgileri alınamadı:', error);
      }
    );
  } else {
    console.error('Yetkilendirme hatası: Oturum açılmamış veya token bulunamadı.');
  }
}

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    const reader = new FileReader();

    reader.onload = (e: any) => {
      this.resizeImage(e.target.result, 100, 100, (resizedImage: string) => {
        this.profileImage = resizedImage;
        console.log('Base64 Kodu:', this.profileImage);
        this.deleteProfilePhoto();
        this.addProfileImage(this.profileImage);
      });
    };

    reader.readAsDataURL(file);
  }

  resizeImage(base64Str: string, maxWidth: number, maxHeight: number, callback: (resizedImage: string) => void) {
    const img = new Image();
    img.src = base64Str;
    img.onload = () => {
      let width = img.width;
      let height = img.height;

      if (width > height) {
        if (width > maxWidth) {
          height *= maxWidth / width;
          width = maxWidth;
        }
      } else {
        if (height > maxHeight) {
          width *= maxHeight / height;
          height = maxHeight;
        }
      }

      const canvas = document.createElement('canvas');
      const ctx = canvas.getContext('2d');
      canvas.width = width;
      canvas.height = height;

      ctx?.drawImage(img, 0, 0, width, height);

      const resizedImage = canvas.toDataURL('image/jpeg');
      callback(resizedImage);
    };
  }

  logout(): void {
    document.cookie = 'jwtToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
    
    window.location.reload();
  }
  
}
