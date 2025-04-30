export const environment = {
  oidc: {
    issuer: 'http://localhost:8080/realms/desafio-verity',
    manageAccountUrl: 'http://localhost:8080/realms/desafio-verity/account',
    clientId: 'webapp',
    scope: 'openid profile email',
    useHttps: false
  },
  apiUrlBase: 'http://localhost:5480'
};
