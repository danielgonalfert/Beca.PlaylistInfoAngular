import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router'; // CLI imports router
import LandingPageView from './components/views/LandingPageView';
import PlaylistListView from './components/views/PlaylistListView';
import DetailedView from './components/views/DetailedView';

const routes: Routes = [
  { path: '', component: LandingPageView },
  { path: 'playlists', component: PlaylistListView },
  { path: 'detailedView/:id', component: DetailedView }
  
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }


