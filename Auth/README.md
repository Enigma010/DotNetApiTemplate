# Authentication
Authentication is handled by the [Authentik](https://goauthentik.io/). 

## Initial Setup
You first need to login to [Autentik Admin](http://localhost:9000). This guide shows how to configure an authentik OAuth2/OpenID Connect application for use with ASP.NET Core.

---

# 1. Create an Application

In the authentik admin panel:

Applications → Applications → Create

Configure:

| Field | Value |
|---|---|
| Name | My .NET App |
| Slug | myapp |

Click **Next**.

---

# 2. Create an OAuth2/OpenID Provider

Choose:

OAuth2/OpenID Provider

Recommended settings:

| Setting | Value |
|---|---|
| Client type | Confidential |
| Authorization flow | default-provider-authorization-implicit-consent |
| Signing key | authentik Self-signed Certificate |

---

# 3. Configure Redirect URIs

Add the ASP.NET Core OpenID Connect callback URL.

For local development:

```text
https://localhost:5001/signin-oidc
```