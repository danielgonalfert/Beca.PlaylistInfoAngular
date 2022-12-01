import { HttpClientModule } from '@angular/common/http';
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

@NgModule({
  declarations: [
    AppComponent, UpperBox, NavBar, PlaylistListBlock,                                          //Components
    LandingPageView, PlaylistListView, DetailedView                                             //Views
  ],
  imports: [
    BrowserModule, HttpClientModule, AppRoutingModule, RouterModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
