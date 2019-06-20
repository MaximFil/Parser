import { ErrorHandler, Injectable, OnInit, Component } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { error } from 'protractor';
import { Http } from '@angular/http';

@Injectable({
  providedIn: 'root',
})
@Component({
  providers: [Http]
})

export class Handle {
  handleError<T>(result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
  }
}
