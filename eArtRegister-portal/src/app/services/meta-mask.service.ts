import { Injectable } from '@angular/core';
import detectEthereumProvider from '@metamask/detect-provider';
import { switchMap } from 'rxjs/operators';
import { from } from 'rxjs';
import { HttpClient } from '@angular/common/http';


interface NonceResponse {
  nonce: string;
}
interface VerifyResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class MetaMaskService {

  constructor(private http: HttpClient) { }

  
  public signInWithMetaMask() {
    let ethereum: any;
    return from(detectEthereumProvider()).pipe(
      // Step 1: Request (limited) access to users ethereum account      
      switchMap(async (provider) => {
        if (!provider) {
          throw new Error('Please install MetaMask');
        }
        ethereum = provider;
        return await ethereum.request({
          method: 'wallet_requestPermissions',
          params: [{ eth_accounts: {} }],
        });
      }),
      // Step 2: Retrieve the current nonce for the requested address
      // switchMap(() =>
      //   this.http.post<NonceResponse>(
      //     'https://us-central1-ionic-angular-web3.cloudfunctions.net/getNonceToSign',
      //     {
      //       address: ethereum.selectedAddress,
      //     }
      //   )
      // ),
      // Step 3: Get the user to sign the nonce with their private key
      // switchMap(
      //   async (response) =>
      //     await ethereum.request({
      //       method: 'personal_sign',
      //       params: [
      //         `0x${this.toHex(response.nonce)}`,
      //         ethereum.selectedAddress,
      //       ],
      //     })
      // ),
      // Step 4: If the signature is valid, retrieve a custom auth token for Firebase
      // switchMap((sig) =>
      //   this.http.post<VerifyResponse>(
      //     'https://us-central1-ionic-angular-web3.cloudfunctions.net/verifySignedMessage',
      //     { address: ethereum.selectedAddress, signature: sig }
      //   )
      // ),
      // Step 5: Use the auth token to auth with Firebase
      // switchMap(
      //   async (response) =>
      //     await signInWithCustomToken(this.auth, response.token)
      // )

    );
  }

  isConnected(){
    let ethereum: any;
    return from(detectEthereumProvider()).pipe(
      // Step 1: Request (limited) access to users ethereum account      
      switchMap(async (provider) => {
        ethereum = provider;
        const accounts = await ethereum.request({method: 'eth_accounts'});   
        return {accounts: accounts, provider: ethereum}
      }),
    );
  }

  private toHex(stringToConvert: string) {
    return stringToConvert
      .split('')
      .map((c) => c.charCodeAt(0).toString(16).padStart(2, '0'))
      .join('');
  }
}
