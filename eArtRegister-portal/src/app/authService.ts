import { Injectable } from "@angular/core";
import { KeycloakService } from "keycloak-angular";
import { environment } from "../environments/environment";

@Injectable({ providedIn: 'root' })
export class AuthService {
    constructor (private keycloakService: KeycloakService) {}

    logout() {
        this.keycloakService.logout("http://localhost:4200/");
    }

    getLoggedUser() {
        try {
            let userDetails = this.keycloakService.getKeycloakInstance().idTokenParsed;
            return userDetails;
        } catch (e) {
            console.log('getLoggedUser Exception', e);
            return undefined;
        }
    }

    getRoles(): string[] {
        return this.keycloakService.getUserRoles();
    }

    getUsername(): string {
        return this.keycloakService.getUsername();
    }

    getToken() {
        return (this.keycloakService.getToken().then((data => {
            if (data) {
                return data;
            }})));
    }
}
