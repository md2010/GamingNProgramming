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

  submitCode(code : any) {
    JSON.stringify(code);
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`)
      .set('Accept','application/json');
  
      return this.http.post<any>(this.apiUrl + '/submit-code', code, {headers: headersToSend, observe : 'response'});
  }

  saveMap(map : any) {
    JSON.stringify(map);
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`)
      .set('Accept','application/json');
  
      return this.http.post<any>(this.apiUrl + '/save-map', map, {headers: headersToSend, observe : 'response'});
  }

  updateMap(map : any) {
    JSON.stringify(map);
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`)
      .set('Accept','application/json');
  
      return this.http.post<any>(this.apiUrl + '/update-map', map, {headers: headersToSend, observe : 'response'});
  }

  updateScoredPoints(data: any) {
    JSON.stringify(data);
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`)
      .set('Accept','application/json');
  
      return this.http.post<any>(this.apiUrl + '/update-scored-points', data, {headers: headersToSend, observe : 'response'});
  }

  getMapForEditing(id: string) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`)
  
      return this.http.get<any>(this.apiUrl + '/get-map-edit/' + id, {headers: headersToSend, observe : 'response'});
  }

  getMaps(id: string) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`)
  
      return this.http.get<any>(this.apiUrl + '/get-map/' + id, {headers: headersToSend, observe : 'response'});
  }
  getMap(id: string) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`)
  
      return this.http.get<any>(this.apiUrl + '/map/' + id, {headers: headersToSend, observe : 'response'});
  }
  getTask(id: string) {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`)
  
      return this.http.get<any>(this.apiUrl + '/task/' + id, {headers: headersToSend, observe : 'response'});
  }
  getPlayerTask(playerId: string, mapId : string, taskId : string = '') {
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`)

    if(taskId !== '') {
      return this.http.get<any>(this.apiUrl + '/player-task/' + playerId + '/' + mapId +  '?' + 'taskId=' + taskId, {headers: headersToSend, observe : 'response'});
    }
    else {
      return this.http.get<any>(this.apiUrl + '/player-task/' + playerId + '/' + mapId, {headers: headersToSend, observe : 'response'});
    }    
  }

  insertPlayerTask(playerTask : any) {
    JSON.stringify(playerTask);
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set( 'Authorization', `Bearer ${this.authService.token}`)
      .set('Accept','application/json');
  
      return this.http.post<any>(this.apiUrl + '/insert-player-task', playerTask, {headers: headersToSend, observe : 'response'});
  }
 
}


