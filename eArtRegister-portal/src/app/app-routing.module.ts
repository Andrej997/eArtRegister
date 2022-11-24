import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BundleComponent } from './components/bundle/bundle.component';
import { CreateBundleComponent } from './components/create-bundle/create-bundle.component';
import { DepositComponent } from './components/deposit/deposit.component';
import { HomeComponent } from './components/home/home.component';
import { MintNftComponent } from './components/mint-nft/mint-nft.component';
import { MyBundlesComponent } from './components/my-bundles/my-bundles.component';
import { MyNftsComponent } from './components/my-nfts/my-nfts.component';
import { MySystemHistoryComponent } from './components/my-system-history/my-system-history.component';
import { NftProfileComponent } from './components/nft-profile/nft-profile.component';
import { SerOnSaleComponent } from './components/ser-on-sale/ser-on-sale.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full',
  },
  {
    path: 'deposit',
    component: DepositComponent,
    pathMatch: 'full',
  },
  {
    path: 'create',
    component: CreateBundleComponent,
    pathMatch: 'full',
  },
  {
    path: 'bundle/:bundleId',
    component: BundleComponent,
    pathMatch: 'full',
  },
  {
    path: 'bundle/:bundleId/mint',
    component: MintNftComponent,
    pathMatch: 'full',
  },
  {
    path: 'bundle/:bundleId/:tokenId',
    component: NftProfileComponent,
    pathMatch: 'full',
  },
  {
    path: 'bundle/:bundleId/:tokenId/set-on-sale',
    component: SerOnSaleComponent,
    pathMatch: 'full',
  },
  // {
  //   path: 'bundles',
  //   component: MyBundlesComponent,
  //   pathMatch: 'full',
  // },
  // {
  //   path: 'nfts',
  //   component: MyNftsComponent,
  //   pathMatch: 'full',
  // },
  {
    path: 'my-history',
    component: MySystemHistoryComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
