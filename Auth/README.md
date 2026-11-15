# Authentication
Authentication is handled by the [Keycloak](https://www.keycloak.org/). 

## Initial Setup
The steps below only need to be done one time after starting keycloak. The finish the setup as defined in the docker configurations on the keycloak side.

1. Go to [local Keycloak](http://localhost:8080/).
1. Login with the credentials you specified for **KEYCLOAK_ADMIN_USERNAME** and **KEYCLOAK_ADMIN_PASSWORD**.
1. Go **Manage realms** and click **Create realm**. For the realm name use the values from the **Solutions/Ddd.App/.env** for the **\$\{APP_DOMAIN\}-\${APP_SUBDOMAIN\}** values.
1. Click on **Clients** and click **Create client**. The client type should be  **OpenID Connect**. For the **Client ID** get the value from **Ddd.App.Web\appsettings.json** for the **WebAuthentication:ClientId** and put it in for the **Client ID**. Set the **Name** to **${APP_DOMAIN}-${APP_SUBDOMAIN}**, click the **Next** button.
1. Turn **Client authentication** to **On**. Turn **Authorization** to **On**. For the **Authentication flow** choose: **Standard flow**, **Implicit flow**, **Standard token exchange**, **JWT Authorization Grant**, **OAuth 2.0 Device Authorization Grant**, **OIDC CIBA Grant**, **Direct access grants**, and **Service account roles**. Click **Next**.
1. Next fill the in the following values:

| Setting | Value |
| - | - |
| Root URL | https://localhost:7035 |
| Home URL | https://localhost:7035 |
| Valid redirect URIs | https://localhost:7035/signin-oidc |
| Valid post logout redirect URIs | https://localhost:7035/signout-callback-oidc |

7. Go to the **Credentials** tab and find the **Client Secret**. This needs to match the value in the  **Solutions/Ddd.App/.env** for **AUTH_CLIENT_SECRET** so either adjust the value in the .env file or in **Keycloak**.
1. Now go to **Users** and click **Create new user**. In the **Username** file in the username.
1. Now go to **Groups** and click **Create group**.
1. Click on the user you just created and click **Credentials** and click **Set password** and set **Temporary** to **Off**.

### Network
To test this locally you'll need to have the **auth** docker compose name route to localhost so that the OIDC redirects work properly. This occurs because the website is running on your local machine and interacting with docker containers.  Within the docker containers the networking routes using the name in the docker compose file.  Keycloak is running the authorization server and in the **docker-compose.yml** file it's called **auth**. The easiest way to get this working locally for development is to change the `C:\Windows\System32\drivers\etc\hosts` and add a line like the following:

```
127.0.0.1 auth
```

## Users
Users are those that have access to your access through the identity provider. To be able to login you'll first need to create users that can login to the application.  To create users go to [local Keycloak](http://localhost:8080), and login with the admin username and password supplied in the **.env** file. Now to go **Users** and click the **Add user**. Fill in the fields necessary to crate the user and click **Create**.  Find the user you've just created and click on the **Credentials** tab and click the **Set password** button.  Set the password and click **Temporary** to **Off**, click the **Set password** button and click **Reset password**.

