import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { JwtValidationService } from './services/JwtValidationService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent  {
  constructor(public dialog: MatDialog,private jwtValidationService: JwtValidationService ) {}
  title = 'NotesApp';
 
   
  
 
  
}
