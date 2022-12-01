import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module'
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import UpperBox from './components/UpperBox.component';
import NavBar from './components/NavBar.component';
import LandingPageView from './components/views/LandingPageView';
import PlaylistListView from './components/views/PlaylistListView';
import PlaylistListBlock from './components/PlaylistListBlock.component';
import DetailedView from './components/views/DetailedView';
import SongListBlock from './components/SongListBlock.component';
import EditForm from './components/EditForm.component';
import EditDetailViewPlaylist from './components/views/EditDetailedViewPlaylist';

@NgModule({
  declarations: [
    AppComponent, UpperBox, NavBar, PlaylistListBlock, SongListBlock,EditForm,                      //Components
    LandingPageView, PlaylistListView, DetailedView, EditDetailViewPlaylist                                      //Views
  ],
  imports: [
    BrowserModule, HttpClientModule, AppRoutingModule, RouterModule, ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
