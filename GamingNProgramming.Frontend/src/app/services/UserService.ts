import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class UserService {

  apiUrl = 'https://localhost:44358/api';

  constructor(private http: HttpClient) { }

  login(credentials: any) {
    JSON.stringify(credentials);
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set("Accept", "application/json");
  
    return this.http.post<any>(this.apiUrl + '/login', credentials, {headers: headersToSend, observe : 'response'});
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('role');
  }

  register(newUser: any) {
    JSON.stringify(newUser);
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set('Accept','application/json');
  
    return this.http.post<any>(this.apiUrl +'/register', newUser, {headers: headersToSend, observe : 'response'});
  }

  getPlayerById(id: any) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${localStorage.getItem('token')}`);
  
    return this.http.get<any>(this.apiUrl + '/user/player/' + id, {headers: headersToSend, observe : 'response'});
  }

}