import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Note } from '../../models/note';
import { AuthenticationService } from '../../services/authentication.service';
import { JwtAuth } from '../../models/jwtAuth';
import { NoteService } from '../../services/note.service';
import { UserService } from '../../services/userService';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'] // styleUrl yerine styleUrls kullanıldı
})
export class HomeComponent implements OnInit {
  @ViewChild('editModal') editModal!: ElementRef;
  noteDto = new Note();
  updateNoteDto = new Note();
  jwtDto = new JwtAuth();
  notes: Note[] = [];
  showModal: boolean = false;
  selectedNote: any = {};

 

  constructor(private authService: AuthenticationService,private noteService: NoteService,private userService:UserService) {}


  ngOnInit(): void {
    this.getNotes();
  }
  createNote(noteDto: Note) {
    const token = this.userService.getToken();
    if (token) {
      this.noteService.createNote(noteDto, token).subscribe(
        response => {
          alert('Not oluşturuldu.');
          this.getNotes();
        },
        error => {
          alert('Not oluşturulurken bir hata oluştu:'+ error.error);
        }
      );
    } else {
      alert('Yetkilendirme hatası: Oturum açılmamış veya token bulunamadı.');
    }
  }

  getNotes(): void {
    const token = this.userService.getToken();
  if (token) {
    this.noteService.getNotes(token).subscribe(
      (notes: Note[]) => {
        this.notes = notes;
      },
      (error) => {
        alert('Notlar alınırken bir hata oluştu:'+ error.error);

      }
    );
  } else {
    alert('Yetkilendirme hatası: Oturum açılmamış veya token bulunamadı.');
  }

  }

  openEditModal(note: Note) {
    this.selectedNote = note;
    this.updateNoteDto.id = note.id
    this.updateNoteDto.title = note.title;
    this.updateNoteDto.content = note.content;
  }

  closeEditModal() {
    this.selectedNote = {};
    this.showModal = false;
  }

  deleteNote(noteId: string): void {
    const token = this.userService.getToken();
    if (token) {
      this.noteService.deleteNoteById(noteId,token)
        .subscribe(() => {
          alert('Not başarıyla silindi.');
          this.notes = this.notes.filter(note => note.id !== noteId);
        }, error => {
          alert('Not silinirken bir hata oluştu:'+ error.error);
        });
    } else {
      alert('Yetkilendirme hatası: Oturum açılmamış veya token bulunamadı.');
    }
  }

  editNote(noteId: string, updatedNote: Note): void {
    const token = this.userService.getToken();
    if (token) {
      this.noteService.editNoteById(noteId, updatedNote, token)
        .subscribe(() => {
         alert('Not başarıyla güncellendi.');
          this.getNotes();
          this.showModal = false;
        }, error => {
          alert('Not güncellenirken bir hata oluştu:' +error.error);
        });
    } else {
      alert('Yetkilendirme hatası: Oturum açılmamış veya token bulunamadı.');
    }
  }
  
  
  
}
