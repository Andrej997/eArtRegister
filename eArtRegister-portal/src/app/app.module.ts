import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { WalletComponent } from './components/wallet/wallet.component';
import { HomeComponent } from './components/home/home.component';
import { BundleComponent } from './components/bundle/bundle.component';
import { CreateBundleComponent } from './components/create-bundle/create-bundle.component';
import { MintNftComponent } from './components/mint-nft/mint-nft.component';
import { MyBundlesComponent } from './components/my-bundles/my-bundles.component';
import { MyNftsComponent } from './components/my-nfts/my-nfts.component';
import { MySystemHistoryComponent } from './components/my-system-history/my-system-history.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    WalletComponent,
    HomeComponent,
    BundleComponent,
    CreateBundleComponent,
    MintNftComponent,
    MyBundlesComponent,
    MyNftsComponent,
    MySystemHistoryComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ToastrModule.forRoot(),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
