import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './AuthService';

@Injectable({
  providedIn: 'root'
})

export class UserService {

  apiUrl = 'https://localhost:44358/api';

  constructor(private http: HttpClient, private authService: AuthService) { }

  //professor
  getProfessorById(id: any) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`);
  
    return this.http.get<any>(this.apiUrl + '/user/professor/' + id, {headers: headersToSend, observe : 'response'});
  }

  getProfessorNotStudents(search: any) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`);
  
    var params = this.getParams(search);
    return this.http.get<any>(this.apiUrl + '/user/not-students?' + params, {headers: headersToSend, observe : 'response'});
  }
  
  addStudent(id: any) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`);
  
      return this.http.post<any>(this.apiUrl + '/user/add-student/' + id, null, {headers: headersToSend, observe : 'response'});
  }

  deleteStudent(id: any) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`);
  
      return this.http.delete<any>(this.apiUrl + '/user/delete-student/' + id, {headers: headersToSend, observe : 'response'});
  }


  //player and friends
  addFriend(id: any) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`);
  
      return this.http.post<any>(this.apiUrl + '/user/add-friend/' + id, null, {headers: headersToSend, observe : 'response'});
  }

  deleteFriend(id: any) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`);
  
      return this.http.delete<any>(this.apiUrl + '/user/delete-friend/' + id, {headers: headersToSend, observe : 'response'});
  }

  getPlayerById(id: any) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`);
  
    return this.http.get<any>(this.apiUrl + '/user/player/' + id, {headers: headersToSend, observe : 'response'});
  }

  getPlayers(search: any) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`);

    var params = this.getParams(search);
    return this.http.get<any>(this.apiUrl + '/user/players?' + params, {headers: headersToSend, observe : 'response'});
  }

  getPlayersFriends(search: any) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`);

    var params = this.getParams(search);
    return this.http.get<any>(this.apiUrl + '/user/friends?' + params, {headers: headersToSend, observe : 'response'});
  }

  getPlayersNotFriends(search: any) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`);
  
    var params = this.getParams(search);
    return this.http.get<any>(this.apiUrl + '/user/not-friends?' + params, {headers: headersToSend, observe : 'response'});
  }
 
  public getParams(search: Search) : string{
    let params = '';
    if(search?.sortOrder?.length > 0) {
      params += `sortOrder=${search.sortOrder}`;
    }
    if(search?.username?.length > 0) {
      if(params.length > 0) {
        params += '&';
      }
      params += `username=${search.username}`;
    }
    if(search?.includeUser != null) {
      if(params.length > 0) {
        params += '&';
      }
      params += `includeUser=${search.includeUser}`;
    }
    if(search?.professorId != null) {
      if(params.length > 0) {
        params += '&';
      }
      params += `professorId=${search.professorId}`;
    }
    return params;
  }
}

interface Search {
  sortOrder : string,
  username : string,
  includeUser: string,
  professorId: string
}

