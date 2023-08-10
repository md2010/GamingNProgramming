import { Component, NgModule } from "@angular/core";
import {FormsModule} from '@angular/forms';
import { CommonModule } from "@angular/common";
import { UserService } from "../services/UserService";
import { ToastComponentComponent } from "../toast-component/toast-component.component";
import { SpinnerComponentComponent } from "../spinner-component/spinner-component.component";
import { Router, ActivatedRoute } from "@angular/router";
import { AuthService } from "../services/AuthService";

@Component({
    selector: 'app-login',
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.css'],
    standalone: true,
    imports: [FormsModule, ToastComponentComponent, CommonModule, SpinnerComponentComponent],
    providers: [UserService]
})

export class LoginComponent {
    title = 'Login';
    username = '';
    password = '';
    message = '';
    showToast = false;
    loading = false;

    constructor(private authService: AuthService, private router: Router, private activatedRoute: ActivatedRoute) {};

    onToastMessageElapsed() {
        this.showToast = false;
    }

    goToRegister() {
        this.router.navigate(['/register']);      
    }

    public validateLogin() {
        this.loading = true;
        const credentialsData = {
            username: this.username,
            password: this.password,
        };
        this.authService.login(credentialsData)
            .subscribe(
                (Response) => {
                if(Response.status == 200) {
                    var data = Response.body;
                    this.authService.storeData(data); 
                    if(data.roleName === 'Student') {
                        this.router.navigate(['/student-dashboard'])
                        .then(() => {
                            window.location.reload();
                          });
                    }
                    else {
                        this.router.navigate(['/professor-dashboard'])
                        .then(() => {
                            window.location.reload();
                          });
                    }

                    this.loading = false;                
                }            
            },
            (error: any) => {
                console.log(error); 
                this.loading = false;
                this.message = "Wrong credentials."
                this.showToast = true;
            }
        );
    }
    
}