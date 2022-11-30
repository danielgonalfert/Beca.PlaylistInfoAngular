import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable , Subscription, tap} from "rxjs";
import  Playlist  from '../models/Playlist.model';


@Component({
  selector: 'playlistListBlock',
  templateUrl: './templates/PlaylistListBlock.component.html',
  styleUrls: ['./styles/PlaylistListBlock.component.css']
})

export default class PlaylistListBlock implements OnInit, OnDestroy {

  private url = 'https://localhost:7035/api/playlists';
  playlists: Playlist[] = [];
  errorMessage = "";
  sub!: Subscription;

  constructor(private http: HttpClient) {}

  getProducts(): Observable<Playlist[]> {
    return this.http.get<Playlist[]>(this.url).pipe(
      tap(data => console.log('All', JSON.stringify(data))));
  }

  ngOnInit(): void {
     this.getProducts().subscribe({
      next: list => this.playlists = list,
      error: err => this.errorMessage = err
     });

    console.log(this.playlists, this.errorMessage);
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
