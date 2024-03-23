import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { of } from 'rxjs/internal/observable/of';
import { throwError } from 'rxjs/internal/observable/throwError';
import { catchError } from 'rxjs/internal/operators/catchError';
import { map } from 'rxjs/internal/operators/map';
import { JwtAuth } from '../models/jwtAuth';

interface UserInfo {
  email: string;
  userName: string;
}
@Injectable({
  providedIn: 'root'
})
export class UserService {
  private tokenKey = 'jwtToken';
  apiUrl = 'http://localhost:5134/api';
  userdata=[];
  constructor(private http: HttpClient) { }

  getToken(): string | null {
    const name = 'jwtToken';
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2 && parts[1] !== undefined) {
      const token = parts[1].split(';').shift();
      if (token !== undefined) {
        return token;
      }
    }
    return null;
  }
  
  getUserInfoFromToken(token: string): UserInfo | null {
    try {
      const payload = token.split('.')[1];
      const decodedPayload = JSON.parse(atob(payload));
      return {
        email: decodedPayload.email,
        userName: decodedPayload.username
      };
    } catch (error) {
      console.error('Token decode hatası:', error);
      return null;
    }
  }
  getUserInfo(token: string): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` 
    });
  
    return this.http.get<UserInfo>(`${this.apiUrl}/AuthManagement/GetProfile`, { headers }).pipe(
      map(response => ({
        userName: response.userName,
        email: response.email
      })),
      catchError(error => {
        console.error('Kullanıcı bilgileri alınamadı:', error);
        return of(null); 
      })
    );
  }
  getUserRoles(token: string, userId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Admin/GetUserRolesById?userId=${userId}`);
  }
  
  
  addUserRole(token: string, userId: string, roleName: string): Observable<any> {
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
    });

    return this.http.post<any>(
        `${this.apiUrl}/Admin/AddUserToRole?userId=${userId}&roleName=${roleName}`, 
        null, 
        { headers }
    );
} 
  changeUsername(newUsername: string, token: string): Observable<any> {
    const userInfo = this.getUserInfoFromToken(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` 
    });
  
    if (userInfo) {
      userInfo.userName = newUsername;
      const updateUrl = `${this.apiUrl}/AuthManagement/ChangeUserName`;
      return this.http.patch<any>(updateUrl, userInfo, { headers });
    } else {
      return throwError('Token ile kullanıcı bilgileri alınamadı');
    }
  }
  changeEmail(newEmail: string, token: string): Observable<any> {
    const userInfo = this.getUserInfoFromToken(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` 
    });
  
    if (userInfo) {
      userInfo.email = newEmail;
      const updateUrl = `${this.apiUrl}/AuthManagement/ChangeEmail`;
      return this.http.patch<any>(updateUrl, userInfo, { headers });
    } else {
      return throwError('Token ile kullanıcı bilgileri alınamadı');
    }
  }
  changePassword(recentPassword:string,newPassword:string, token: string): Observable<any> {
    const userInfo = this.getUserInfoFromToken(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` 
    });
    const body = {
      currentPassword: recentPassword,
      newPassword: newPassword
    };
      const updateUrl = `${this.apiUrl}/AuthManagement/ChangePassword`;
      return this.http.patch<any>(updateUrl, body, { headers });
    
  }

  addProfileImage(token: string, profileImage: string): Observable<any> 
  {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` 
    });
    const encodedBase64 = encodeURIComponent(profileImage);

    const queryString = `?base64=${encodedBase64}`;

    const addImageUrl = `${this.apiUrl}/AddProfilePhoto${queryString}`;

    return this.http.post(addImageUrl, null, { headers });
  }
  
  getProfileImage(token: string): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` 
    });
  
    return this.http.get(`${this.apiUrl}/GetProfilePhoto`, { headers, responseType: 'text' }).pipe(
      catchError(error => {
        if (error.status === 404) {
          console.log('Kullanıcı profil fotoğrafı bulunamadı.');
          return of(null);
        } else {
          console.error('Profil fotoğrafı alınırken bir hata oluştu:', error);
          return throwError(error);
        }
      })
    );
  }
  
  
  deleteProfileImage(token: string): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  
    return this.http.delete(`${this.apiUrl}/DeleteProfilePhoto`, { headers })
      .pipe(
        catchError(error => {
          console.error('Profil fotoğrafı silinirken bir hata oluştu:', error);
          throw error; 
        })
      );
  }
  getAllUsers(token: string): Observable<any>{
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.get(`${this.apiUrl}/Admin/AllUsers`, { headers });
  }

  checkLockout(userId: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/Islockedout/${userId}`);
  }

  lockUser(token: string):Observable<any>{
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put(`${this.apiUrl}/AuthManagement/Deactivate`, { headers });
  }


  unLockUser(token: string, userId: string):Observable<any>{
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put(`http://localhost:5134/${userId}api/AuthManagement/Activate`, { headers });
  }
  lockUserById(token: string, userId: string):Observable<any>{
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put(`http://localhost:5134/${userId}api/AuthManagement/DeactivateById`, { headers });
  }
}
