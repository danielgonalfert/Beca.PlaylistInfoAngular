
export default class Song {
  id: number;
  title: string;
  description: string;

  constructor(_id: number, _title: string, _desc: string) {
    this.id = _id;
    this.title = _title;
    this.description = _desc
  }
}
