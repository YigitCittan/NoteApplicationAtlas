import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../services/authentication.service';
import { Login } from '../../models/login';
import { Register } from '../../models/register';
import { JwtAuth } from '../../models/jwtAuth';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  title = 'NotesApp';
  loginDto = new Login();
  registerDto = new Register();
  jwtDto = new JwtAuth();
  
  
  constructor(private authService: AuthenticationService ,private router: Router) {}
login(loginDto: Login) {
    this.authService.login(loginDto).subscribe(
        (jwtDto) => {

            document.cookie = `jwtToken=${jwtDto.token}; SameSite=Strict; path=/`;
            if (jwtDto.result) {
                this.router.navigateByUrl('/home');
            }
        },
        (error) => {
            const errorMessage =  'Kullanıcı adı veya parola hatalı.';
            alert('Giriş hatası: ' + errorMessage);
        }
    );
}
register(registerDto: Register){
    this.authService.register(registerDto).subscribe(
        (jwtDto) =>{

            document.cookie = `jwtToken=${jwtDto.token}; SameSite=Strict; path=/`;
            if (jwtDto.result) {
                this.router.navigateByUrl('/home');
            }
        },
        (error) => {
            const errorMessage =  'Kullanıcı kaydedilemedi Uygunsuz içerik.';
            alert('Kayıt hatası: ' + errorMessage);
        }
    );
        }
    
}

