import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BundleComponent } from './components/bundle/bundle.component';
import { CreateBundleComponent } from './components/create-bundle/create-bundle.component';
import { HomeComponent } from './components/home/home.component';
import { MintNftComponent } from './components/mint-nft/mint-nft.component';
import { MyBundlesComponent } from './components/my-bundles/my-bundles.component';
import { MyNftsComponent } from './components/my-nfts/my-nfts.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full',
  },
  {
    path: 'bundles/:bundleId',
    component: BundleComponent,
    pathMatch: 'full',
  },
  {
    path: 'bundles',
    component: MyBundlesComponent,
    pathMatch: 'full',
  },
  {
    path: 'nfts',
    component: MyNftsComponent,
    pathMatch: 'full',
  },
  {
    path: 'bundles/:bundleId/mint',
    component: MintNftComponent,
    pathMatch: 'full',
  },
  {
    path: 'create',
    component: CreateBundleComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
