import { KeycloakService} from 'keycloak-angular';
// import { environment } from './environments/environment';

export function initializer(keycloak: KeycloakService): () => Promise<void> {
    return (): Promise<void> => {
        return new Promise(async (resolve, reject) => {
          try {
            await keycloak.init({
                config: {
                    url: "http://localhost:4200/auth", //environment.keycloak.url,
                    realm: "eart_register",//environment.keycloak.realm,
                    clientId: "eart_portal"//environment.keycloak.clientId
                },
              loadUserProfileAtStartUp: false,
              initOptions: {
                onLoad: 'login-required',
                checkLoginIframe: true
              },
              bearerExcludedUrls: []
            });
            resolve();
          } catch (error) {
            reject(error);
          }
        });
      };
}
