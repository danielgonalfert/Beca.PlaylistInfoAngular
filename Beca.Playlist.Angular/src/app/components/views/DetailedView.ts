import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, Subscription, tap } from "rxjs";
import Playlist from '../../models/Playlist.model';

@Component({
  selector: 'playlistListBlock',
  templateUrl: './templates/DetailedView.html',
  styleUrls: ['./styles/DetailedView.css']
})

export default class DetailedView  {

 
}
