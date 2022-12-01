import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, Subscription, tap } from "rxjs";
import Song from '../models/Playlist.model';


@Component({
  selector: 'songListBlock',
  templateUrl: './templates/SongListBlock.component.html',
  styleUrls: ['./styles/SongListBlock.component.css']
})

export default class SongListBlock  {

  @Input()
  id!: number;

  private url = 'https://localhost:7035/api/playlists';
  songs:Song[] = [];
  errorMessage = "";
  sub!: Subscription;

  constructor(private http: HttpClient) { }

  getProducts(): Observable<Song[]> {
    return this.http.get<Song[]>(this.url + this.id).pipe(
      tap(data => console.log('All', JSON.stringify(data))));
  }

  ngOnInit(): void {
    this.getProducts().subscribe({
      next: list => this.songs = list,
      error: err => this.errorMessage = err
    });

    console.log(this.songs, this.errorMessage);
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
