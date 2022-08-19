# eArtRegister

- Wallet
kreirati 3 novcanika (server, prodavac, kupac)
za svaki novcanik dodati mrezu sa sledecim podacima:
* Network Name: Alchemy Rinkeby
* New RPC URL: https://eth-rinkeby.alchemyapi.io/v2/n48gxDqKZegDyS7Vxao0tiAz4wtuudm1
* Chain ID: 4
* Currency Symbol: ETH

- Front 
otvoriti 'eArtRegister\eArtRegister-portal' cmd 
obezbediti slobodan port 4200
npm install --save-dev @angular-devkit/build-angular
ng serve

- API
na lokaciji (po potrebi) '\eArtRegister\eArtRegister-api\eArtRegister.API\src\WebApi\appsettings.json' promenuti podatke za sledece parametre:
* "ConnectionStrings__DefaultConnection"
* "Nethereum__ServerWallet"
* "Nethereum__PrivateKey"
otvoriti projekat u visual studio 2022 (ne moze niza verzija jer je .NET6)
WebApi 'Set as Startup Project'

-IPFS docker 
docker run -d --name ipfs_host -p 4001:4001 -p 4001:4001/udp -p 127.0.0.1:9090:8080 -p 127.0.0.1:5001:5001 ipfs/go-ipfs:latest

