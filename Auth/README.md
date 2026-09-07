# Authentication
Authentication is handled by the [Keycloak](https://www.keycloak.org/). 

## Initial Setup
See [Setup > Authentication](../Docs/Procedures/Setup/README.md#appevents-setup) for initial setup.

# Users
Users are those that have access to your access through the identity provider. To be able to login you'll first need to create users that can login to the application.  To create users go to [local Keycloak](http://localhost:8080), and login with the admin username and password supplied in the **.env** file. Now to go **Users** and click the **Add user**. Fill in the fields necessary to crate the user and click **Create**.  Find the user you've just created and click on the **Credentials** tab and click the **Set password** button.  Set the password and click **Temporary** to **Off**, click the **Set password** button and click **Reset password**.

# Roles
Roles are permissions that a specific user has on the system.  If you needd 