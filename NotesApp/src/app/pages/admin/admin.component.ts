import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/userService';
import { User } from '../../models/user';
import { JwtAuth } from '../../models/jwtAuth';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  // Varsayılan olarak dashboard sayfası
  users: User[] = [];
  selectedUser: any = {};
  userDto = new User();
  showModal: boolean = false;
  isLockedOut: boolean | undefined;
  jwtDto = new JwtAuth();

  constructor(private router: Router, private userService: UserService) {}

  ngOnInit(): void {
    this.getAllUsers();
  }

  
  getAllUsers(): void {
    const token = this.userService.getToken();

  // Token varsa notları alma isteğini gönder
  if (token) {
    this.userService.getAllUsers(token).subscribe(
      (users: User[]) => {// Notları konsola yazdır
      this.users=users;
      },
      (error) => {
        alert('Kullanıcılar alınırken bir hata oluştu: ' + error.error);
      }
    );
  } else {
    alert('Yetkilendirme hatası: Oturum açılmamış veya token bulunamadı.');
  }

  }
  
  



  
openEditModal(user: User) {
  this.selectedUser = user;
  this.userDto.id= user.id;
  this.userDto.userName=user.userName;
  this.userDto.email=user.email;
  this.checkLockout();
}
closeEditModal() {
  this.selectedUser = {};
  this.showModal = false;
}


checkLockout(): void {
  if (this.selectedUser && this.selectedUser.id) {
    this.userService.checkLockout(this.selectedUser.id)
      .subscribe((result) => {
        this.isLockedOut = result;
        console.log('Is Locked Out:', result);
      });
  }
}
deactiveUser() {
  const token = this.userService.getToken();
  if (token) {
    const userId = this.selectedUser.id;
    this.userService.lockUserById(token,userId)
      .subscribe(
        response => {
          console.log('Kullanıcı kilitleme isteği başarılı:', response);
          alert('Kullanıcı kilitleme isteği başarılı');
          
        },
        error => {
          console.error('Kullanıcı kilitleme isteği hatası:', error);
          alert('Kullanıcı kilitleme isteği hatası:'+ error);
        }
      );
  } else {
    console.error('Token bulunamadı!');
  }
}



activateUser(){
  const token = this.userService.getToken();
  if (token) {
    const userId = this.selectedUser.id;
  this.userService.unLockUser(token,userId).subscribe(
    response => {
      console.log('Kullanıcı kilitleme isteği başarılı:', response);
      alert('Kullanıcı kilit açma isteği başarılı');
    },
    error => {
      console.error('Kullanıcı kilit açma isteği hatası:', error);
      alert('Kullanıcı kilit açma isteği hatası:' +error.error);

    }
  );
  } else {
  console.error('Token bulunamadı!');
  }
}
}

