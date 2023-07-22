import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './AuthService';

@Injectable({
  providedIn: 'root'
})

export class GameService {

  apiUrl = 'https://localhost:44358/api/game';

  constructor(private http: HttpClient, private authService: AuthService) { }

  runCode(code : any) {
    JSON.stringify(code);
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`)
      .set('Accept','application/json');
  
      return this.http.post<any>(this.apiUrl + '/run-code', code, {headers: headersToSend, observe : 'response'});
  }

  saveMap(map : any) {
    JSON.stringify(map);
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`)
      .set('Accept','application/json');
  
      return this.http.post<any>(this.apiUrl + '/save-map', map, {headers: headersToSend, observe : 'response'});
  }
 
}


