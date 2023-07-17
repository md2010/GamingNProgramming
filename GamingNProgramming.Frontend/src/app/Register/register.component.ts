import { Component, NgModule } from "@angular/core";
import {MatSelectModule} from "@angular/material/select";
import {MatFormFieldModule} from '@angular/material/form-field';
import {FormsModule} from '@angular/forms';
import {Router} from "@angular/router"
import { CommonModule } from "@angular/common";
import { AuthService } from "../services/AuthService";
import {LookupService} from "../services/LookupService";
import { ActivatedRoute } from "@angular/router";

import { SelectAvatarComponent } from "./select-avatar/select-avatar.component";
import { SpinnerComponentComponent } from "../spinner-component/spinner-component.component";
import { ToastComponentComponent } from "../toast-component/toast-component.component";

@Component({
    selector: 'app-register',
    templateUrl: 'register.component.html',
    styleUrls: ['register.component.css'],
    imports: [
        MatSelectModule, 
        MatFormFieldModule, 
        FormsModule, 
        SelectAvatarComponent, 
        CommonModule, 
        SpinnerComponentComponent,
        ToastComponentComponent
    ],
    standalone: true,
    providers: [
        AuthService,
        LookupService
    ]
})

export class RegisterComponent {
    title = 'Register';
    roles = new Array<any>;
    selectedRole = <Role>{};
    username = '';
    email = '';
    password = '';
    avatarId = '';
    newUser = {};
    loading = false;
    showToast = false;
    message = '';

    constructor(private router: Router, private authService: AuthService, private lookupService: LookupService, private activeRoute: ActivatedRoute) { }

    ngOnInit() {
        this.lookupService.getRoles().subscribe(response => {
            this.roles = response;
        })
    }

    goToLogin() {
        this.router.navigate(['login']);
    }

    onAvatarSelect(value: string) {
        this.avatarId = value;
    }

    onToastMessageElapsed() {
        this.showToast = false;
    }

    public onRegisterClick() {
        this.loading = true;
        this.newUser = {
            username: this.username,
            email: this.email,
            password: this.password,
            roleId: this.selectedRole.id,
            roleName: this.selectedRole.name,
            avatarId: this.avatarId
        }
        this.authService.register(this.newUser)
        .subscribe(
            (Response) => {
                if(Response.status == 200) {
                    this.loading = false;
                    this.router.navigate(['login']); 
                }
                else if(Response.status == 409) {
                    this.loading = false;
                    this.message = "already exists."
                    this.showToast = true;
                }
                else {
                    this.loading = false;
                    this.message = "Something went wrong with registration."
                    this.showToast = true;
                }
            },
            (error: any) => {
                console.log(error);
                this.loading = false;
                this.message = error.error;
                this.showToast = true;
            });       
    }
}

interface Role {
    id: string;
    name: string;
}