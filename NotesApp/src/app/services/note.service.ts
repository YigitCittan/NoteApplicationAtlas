import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Note } from '../models/note';
import { Observable } from 'rxjs/internal/Observable';
import { Token } from '@angular/compiler';

@Injectable({
  providedIn: 'root'
})
export class NoteService {
  apiUrl = 'http://localhost:5134/api'; 

  constructor(private http: HttpClient) {}

  public createNote(note: Note, token: string): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.post<any>(`${this.apiUrl}/PostNote`, note, { headers });
  }

  public getNotes(token: string): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` 
    });
    return this.http.get<any>(`${this.apiUrl}/GetNotes`, { headers });
  }

  public getAllNotes(token: string): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<any>(`${this.apiUrl}/GetAllNotes`, { headers });
  }

  public deleteNoteById(noteId: string , token: string): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` 
    });
    const deleteUrl = `${this.apiUrl}/DeleteNote${noteId}`; 
    return this.http.delete<any>(deleteUrl, { headers });
  }

  public editNoteById(noteId: string, updatedNote: Note, token: string): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` 
    });
    const updateUrl = `${this.apiUrl}/UpdateNote${noteId}`;
    return this.http.put<any>(updateUrl, updatedNote, { headers });
  }
}
