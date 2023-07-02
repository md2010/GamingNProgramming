import { Component, NgModule } from "@angular/core";
import {MatSelectModule} from "@angular/material/select";
import {MatFormFieldModule} from '@angular/material/form-field';
import {FormsModule} from '@angular/forms';
import {Router} from "@angular/router"
import { CommonModule } from "@angular/common";
import { UserService } from "../services/UserService";
import {LookupService} from "../services/LookupService";

import { SelectAvatarComponent } from "./select-avatar/select-avatar.component";
import { SpinnerComponentComponent } from "../spinner-component/spinner-component.component";
import { ToastComponentComponent } from "../toast-component/toast-component.component";

@Component({
    selector: 'app-register',
    templateUrl: 'register.component.html',
    styleUrls: ['register.component.css'],
    imports: [
        MatSelectModule, MatFormFieldModule, FormsModule, SelectAvatarComponent, CommonModule, SpinnerComponentComponent,ToastComponentComponent
    ],
    standalone: true,
    providers: [
        UserService,
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

    constructor(private router: Router, private userService: UserService, private lookupService: LookupService) { }

    ngOnInit() {
        this.lookupService.getRoles().subscribe(response => {
            this.roles = response;
        })
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
        this.userService.register(this.newUser)
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