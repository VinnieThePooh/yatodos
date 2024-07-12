import { Component } from '@angular/core';
import { ApiUrls } from '../../app.config';
import { HttpClient } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { catchError, map } from 'rxjs';

@Component({
  selector: 'app-todos',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './todos.component.html',
  styleUrl: './todos.component.css',
})
export class TodosComponent {
  constructor(private httpClient: HttpClient) {}

  authTest() {
    this.httpClient
      .get(ApiUrls.Base + '/api/todos/getuser')
      .subscribe((r) => console.log(r));
  }
}
