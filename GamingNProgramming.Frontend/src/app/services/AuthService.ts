import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { HttpHeaders } from "@angular/common/http";
import { JwtHelperService, JWT_OPTIONS } from "@auth0/angular-jwt";
import { Observable, BehaviorSubject } from "rxjs";
import { Router } from "@angular/router";

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  apiUrl = 'https://localhost:44358/api';

  constructor(private http: HttpClient, private router: Router) {
    var isExpired = this.checkExpiry();
    this.token = sessionStorage.getItem("token");

    if( this.token != null && !isExpired) {
      this.authorizedSubject = new BehaviorSubject<Authorized>({ 
        isAuth : true, 
        userId: sessionStorage.getItem("userId"), 
        roleName : sessionStorage.getItem("role"),
        token: sessionStorage.getItem("token")
      });
    }
    else {
      this.authorizedSubject = new BehaviorSubject<Authorized>({
        isAuth: false,
        userId: null, 
        roleName : null,
        token: null
      });
      this.logout();
    }
    this.authorized = this.authorizedSubject.asObservable();
  }

  private jwtHelper = new JwtHelperService();
  private authorizedSubject: BehaviorSubject<Authorized>;
  public authorized: Observable<Authorized>;
  public token : string | null

  timeout : any;
  expiryTimer : any;

  public getAuthorized () {
   return this.authorizedSubject.value;
  }

  login(credentials: any) {
    JSON.stringify(credentials);
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set("Accept", "application/json");
  
    return this.http.post<any>(this.apiUrl + '/login', credentials, {headers: headersToSend, observe : 'response'});
  }

  storeData(data: AuthResponse) {
    this.timeout = this.jwtHelper.getTokenExpirationDate(data.token);
    this.expiryTimer = setInterval(() => { this.checkExpiry(); }, 30000);
    sessionStorage.setItem("token", data.token);
    sessionStorage.setItem("userId", data.userId)
    sessionStorage.setItem('role',data.roleName);
    this.setProperties();
  }

  private checkExpiry(): boolean {
    if(sessionStorage.getItem("token") != null) {
      if (this.jwtHelper.isTokenExpired(sessionStorage.getItem("token"))) {
        return true;
      }
    }
    if(sessionStorage.getItem("token") == null)
      return true;
    return false;
}

  setProperties() {
    this.authorizedSubject.next({
      isAuth : true, 
      userId: sessionStorage.getItem("userId"), 
      roleName : sessionStorage.getItem("role"),
      token: sessionStorage.getItem("token")
    });
    this.token = sessionStorage.getItem("token");
  }

  logout() {
    this.authorizedSubject.next({
      isAuth : false,
      userId: null, 
      roleName : null,
      token: null
    });
    this.token = null;
    sessionStorage.clear();
    this.router.navigate(['login']);;
  }

  register(newUser: any) {
    JSON.stringify(newUser);
    let headersToSend = new HttpHeaders();
    headersToSend = headersToSend
      .set('Accept','application/json');
  
    return this.http.post<any>(this.apiUrl +'/register', newUser, {headers: headersToSend, observe : 'response'});
  }

}

interface AuthResponse {
    userId: string,
    token: string,
    roleName: string
}

interface Authorized {
  token: string | null,
  isAuth : boolean | null,
  roleName: string | null,
  userId: string | null
}