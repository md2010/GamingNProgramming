import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class LookupService {

  apiUrl = 'https://localhost:44358/api/lookup';

  constructor(private http: HttpClient) { }

  public getRoles() : Observable<any>{
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set('Accept','application/json');
  
    return this.http.get<any>(this.apiUrl +'/roles', {headers: headersToSend});
  }

  public getAvatars() : Observable<any>{
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set('Accept','application/json');
  
    return this.http.get<any>(this.apiUrl +'/avatars', {headers: headersToSend});
  }

  public getBadges() : Observable<any>{
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set('Accept','application/json');
  
    return this.http.get<any>(this.apiUrl +'/badges', {headers: headersToSend});
  }

}