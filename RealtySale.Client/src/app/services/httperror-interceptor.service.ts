import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, concatMap, Observable, of, retryWhen, throwError } from "rxjs";
import { ErrorCode } from "../enums/ErrorCode.enum";
import { AlertifyService } from "./alertify.service";

@Injectable({
  providedIn: 'root'
})
export class HttpErrorInterceptorService implements HttpInterceptor {

  constructor(private alertify: AlertifyService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      retryWhen(error => this.retryRequest(error, 10)),
      catchError((error: HttpErrorResponse) => {
        const errorMessage = this.setError(error);
        console.log(error);
        this.alertify.error(errorMessage);
        return throwError(errorMessage);
      })
    );
  }

  retryRequest(error: Observable<HttpErrorResponse>, retryCount: number): Observable<unknown> {
    return error.pipe(
      concatMap((checkErr: HttpErrorResponse, count: number) => {
        if (count <= retryCount) {
          switch(checkErr.status) {
            case ErrorCode.serverDown: return of(checkErr);
          }
        }
        return throwError(checkErr);
      })
    );
  }

  setError(error: HttpErrorResponse): string {
    let errorMessage = 'Unknown error occured';

    if (error.error instanceof ErrorEvent) {
      // Client side errors
      errorMessage = error.error.message;
    } else {
      // Server side errors
      if (error.status === ErrorCode.unauthorised) {
        return error.statusText;
      }

      if (error.error.errorMessage && error.status !== ErrorCode.serverDown) {
        { errorMessage = error.error.errorMessage; }
      }
    }

    return errorMessage;
  }
}
