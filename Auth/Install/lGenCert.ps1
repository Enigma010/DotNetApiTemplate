# Generates local self signed certificates for use in keycloak these are required to be run befor the docker compose command runs.
New-Item -ItemType Directory -Force -Path "../../Output/Certs/Local" | Out-Null
mkcert -install
mkcert -key-file ../../Output/Certs/Local/auth-dev.key -cert-file ../../Output/Certs/Local/auth-dev.crt localhost auth auth.local 127.0.0.1
# Copies the root CA to the output directory so it can be copied to the docker containers for the website and the api, in particular
# this was needed in the website to redirect to keycloak authentication as if the the site wasn't secure and trusted the redirect
# would fail
$rootCaDir = mkcert -CAROOT
New-Item -Path "../../Output/Certs/RootCa" -ItemType Directory -Force | Out-Null
copy $rootCaDir/rootCA.pem ../../Output/Certs/RootCa/mkcert-rootCA.crt