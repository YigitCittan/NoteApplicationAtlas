import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './pages/login/login.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { LayoutComponent } from './pages/layout/layout.component';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MatDialogModule } from '@angular/material/dialog';
import { ChangePasswordModalComponent } from './pages/profile/modals/change-password-modal/change-password-modal.component';
import { ChangeEmailModalComponent } from './pages/profile/modals/change-email-modal/change-email-modal.component';
import { ChangeUsernameModalComponent } from './pages/profile/modals/change-username-modal/change-username-modal.component';
import { AuthenticationInterceptor } from './services/interceptor';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { AdminComponent } from './pages/admin/admin.component';
import { HomeComponent } from './pages/home/home.component';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ProfileComponent,
    AdminComponent,
    LayoutComponent,
    ChangePasswordModalComponent,
    ChangeEmailModalComponent,
    ChangeUsernameModalComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    MatDialogModule,
    CommonModule,
    MatSelectModule
  ],
  providers: [
    provideClientHydration(),
    provideAnimationsAsync('noop'),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
