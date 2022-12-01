import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router'; // CLI imports router
import LandingPageView from './components/views/LandingPageView';
import PlaylistListView from './components/views/PlaylistListView';
import DetailedView from './components/views/DetailedView';
import EditDetailViewPlaylist from './components/views/EditDetailedViewPlaylist';

const routes: Routes = [
  { path: '', component: LandingPageView },
  { path: 'playlists', component: PlaylistListView },
  { path: 'playlists/detailedView/:id', component: DetailedView },
  { path: 'playlists/editDetailViewPlaylist/:id', component: EditDetailViewPlaylist}
  
];


@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }


