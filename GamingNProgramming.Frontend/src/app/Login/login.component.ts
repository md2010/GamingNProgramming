import { Component, NgModule } from "@angular/core";
import {FormsModule} from '@angular/forms';
import { CommonModule } from "@angular/common";
import { UserService } from "../services/UserService";
import { ToastComponentComponent } from "../toast-component/toast-component.component";
import { Router } from "@angular/router";

@Component({
    selector: 'app-login',
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.css'],
    standalone: true,
    imports: [FormsModule, ToastComponentComponent, CommonModule],
    providers: [UserService]
})

export class LoginComponent {
    title = 'Login';
    username = '';
    password = '';
    message = '';
    showToast = false;

    constructor(private userService: UserService, private router: Router) {};

    onToastMessageElapsed() {
        this.showToast = false;
    }

    public validateLogin() {
        const credentialsData = {
            username: this.username,
            password: this.password,
        };
        this.userService.login(credentialsData)
        .subscribe(
            (Response) => {
            if(Response.status == 200) {
                var data = Response.body;
                localStorage.setItem('token',data.token);
                localStorage.setItem('userId',data.userId);
                localStorage.setItem('role',data.roleName);
                this.router.navigate(['/student-dashboard']);
            }            
        },
        (error: any) => {
            console.log(error); 
            this.message = "Wrong credentials."
            this.showToast = true;
        }
    )}
    
}