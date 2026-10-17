# Authentication
Authentication is handled by the [Keycloak](https://www.keycloak.org/). 

## Initial Setup
See [Setup > Authentication](../Docs/Procedures/Setup/README.md#appevents-setup) for initial setup.

# Users
Users are those that have access to your access through the identity provider. To be able to login you'll first need to create users that can login to the application.  To create users go to [local Keycloak](http://localhost:8080), and login with the admin username and password supplied in the **.env** file. Now to go **Users** and click the **Add user**. Fill in the fields necessary to crate the user and click **Create**.  Find the user you've just created and click on the **Credentials** tab and click the **Set password** button.  Set the password and click **Temporary** to **Off**, click the **Set password** button and click **Reset password**.

# Network
To test this locally you'll need to have the **auth** docker compose name route to localhost so that the OIDC redirects work properly. This occurs because the website is running on your local machine and interacting with docker containers.  Within the docker containers the networking routes using the name in the docker compose file.  Keycloak is running the authorization server and in the **docker-compose.yml** file it's called **auth**. The easiest way to get this working locally for development is to change the `C:\Windows\System32\drivers\etc\hosts` and add a line like the following:

```
127.0.0.1 auth
```