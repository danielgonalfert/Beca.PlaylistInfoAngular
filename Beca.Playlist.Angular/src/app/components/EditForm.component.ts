import { FormGroup,Validators,FormControl} from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import Playlist from '../models/Playlist.model';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'editForm',
  templateUrl: './templates/EditForm.component.html',
  styleUrls: ['./styles/EditForm.component.css']
})

export default class EditForm {

  private url = 'https://localhost:7035/api/playlists/update/'

  @Input()
  currentPlaylist!: Playlist;

  


  myForm = new FormGroup({
    id: new FormControl('', Validators.required),
    title: new FormControl('', [Validators.required, Validators.maxLength(25)]),
    description: new FormControl('', [Validators.required, Validators.maxLength(250)])
  });

  

  constructor(private http: HttpClient, private route:Router) {

  }

  

 

  OnSubmit(): void {
    


    this.http.post<any>(this.url + this.currentPlaylist.id, this.myForm.value).subscribe(data => {})
    this.route.navigateByUrl('/playlists');
  }
}
