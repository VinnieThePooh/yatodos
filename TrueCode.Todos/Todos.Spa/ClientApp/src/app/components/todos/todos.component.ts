import { Component, OnInit } from '@angular/core';
import { BaseUrl } from '../../app.config';
import { HttpClient } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { catchError, map } from 'rxjs';
import { TodosService as TodoService } from '../../services/todos.service';

@Component({
  selector: 'app-todos',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './todos.component.html',
  styleUrl: './todos.component.css',
})
export class TodosComponent implements OnInit
{
  constructor(
    private httpClient: HttpClient,
    private todoService: TodoService) {}  

  authTest() {
    this.httpClient
      .get(BaseUrl + '/api/todos/getuser')
      .subscribe((r) => console.log(r));
  }

  ngOnInit(): void {
    
  }
}
