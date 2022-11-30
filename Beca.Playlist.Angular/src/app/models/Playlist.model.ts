
import Song from './Song.model';

export default class Playlist {
  id: number;
  title: string;
  description: string;
  songs: Song[] = [];

  constructor(_id: number, _title: string, _desc: string, _songs: Song[]) {
    this.id = _id;
    this.title = _title;
    this.description = _desc;
    this.songs = _songs;
  }
}
