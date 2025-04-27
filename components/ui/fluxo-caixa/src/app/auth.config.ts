import { AuthConfig } from "angular-oauth2-oidc";
import { environment } from "../environments/environment";

export const authConfig: AuthConfig = {
  issuer: environment.oidc.issuer,
  redirectUri: window.location.origin,
  clientId: environment.oidc.clientId,
  scope: environment.oidc.scope,
  responseType: 'code',
  requireHttps: environment.oidc.useHttps,
  disablePKCE: false,
};
