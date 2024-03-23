import { HttpEvent, HttpHandler,HttpInterceptor,HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UserService } from "./userService";

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor{
    constructor(private userService: UserService) {}
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.userService.getToken();
        
        if (token) {
            req = req.clone({
                setHeaders: {
                    Authorization: 'Bearer ' + token // Bearer ve token arasında bir boşluk bırakma
                }
            });
        } else {
        }

        return next.handle(req);
    }
    
}
