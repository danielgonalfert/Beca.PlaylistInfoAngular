import { Component, Input, OnInit, OnDestroy } from '@angular/core';

import Song from '../models/Playlist.model';



@Component({
  selector: 'songListBlock',
  templateUrl: './templates/SongListBlock.component.html',
  styleUrls: ['./styles/SongListBlock.component.css']
})

export default class SongListBlock  {

  @Input()
  songs: Song[] = [];
}
