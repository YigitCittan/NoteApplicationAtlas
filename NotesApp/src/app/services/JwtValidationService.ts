import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable, catchError, interval, map, mergeMap, of, switchMap } from 'rxjs';
import { response } from 'express';

@Injectable({
  providedIn: 'root'
})
export class JwtValidationService {
  private apiUrl = 'http://localhost:5134/validate-token'; 
  private intervalMs = 60000;
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
  
  validateTokenPeriodically(): Observable<boolean> {
    const token = this.getToken();
    const tokenValue = JSON.stringify(token);


  const headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`, 
  });

  return interval(this.intervalMs).pipe(
    mergeMap(() => 
      this.http.post<any>(this.apiUrl, tokenValue, { headers: headers, observe: 'response' }).pipe(
        map(response => {
          return response.body;
        }),
        catchError(error => {
          return of(false); 
        })
      )
    )
  );
}
}
