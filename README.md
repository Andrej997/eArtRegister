# eArtRegister

-test node (geth) se pokrece 
D:\MASTER\Pravna informatika\testchain\clique\startgeth.bat

-IPFS docker 
docker run -d --name ipfs_host -p 4001:4001 -p 4001:4001/udp -p 127.0.0.1:9090:8080 -p 127.0.0.1:5001:5001 ipfs/go-ipfs:latest
portal : http://127.0.0.1:5001/ipfs/bafybeihcyruaeza7uyjd6ugicbcrqumejf6uf353e5etdkhotqffwtguva/#/welcome

-KeyCloak docker
docker run -p 10001:8080 -e KEYCLOAK_ADMIN=admin -e KEYCLOAK_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:13.0.0 start-dev