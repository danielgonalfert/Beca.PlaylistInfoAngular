import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, Subscription, tap } from "rxjs";
import Playlist from '../../models/Playlist.model';
import Song from '../../models/Song.model';


@Component({
  selector: 'EditDetailedPlaylist',
  templateUrl: './templates/EditDetailedViewPlaylist.html',
  styleUrls: ['./styles/DetailedView.css']
})

export default class DetailedView {


  id!: string;
  private url = 'https://localhost:7035/api/playlists/id/';

  @Input()
  playlist!: Playlist;
  songs: Song[] = [];

  paramSub!: Subscription;
  sub!: Subscription;
  errorMessage!: string;

  constructor(private http: HttpClient, private route: ActivatedRoute) { }


  getPlaylist(): Observable<Playlist> {
    return this.http.get<Playlist>(this.url + this.id + "?withSongs=true").pipe(
      tap(data => console.log('All', JSON.stringify(data))));
  }


  ngOnInit(): void {

    this.paramSub = this.route.params.subscribe(params => { this.id = params['id']; console.log(this.id) })

    this.sub = this.getPlaylist().subscribe({
      next: p => { this.playlist = p; this.songs = p.songs; console.log(this.songs) },
      error: err => this.errorMessage = err
    });
  }


  ngOnDestroy(): void {

    if (this.paramSub) {
      this.paramSub.unsubscribe();
    }

    if (this.sub) {
      this.sub.unsubscribe();
    }

  }

  deleteSong(id: string): void {
    this.http.delete<any>('https://localhost:7035/api/songs/delete/' + id).subscribe(data => { console.log(data)})
    window.location.reload();
  }
}
