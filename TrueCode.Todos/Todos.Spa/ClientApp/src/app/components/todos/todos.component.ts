import { Component, OnInit } from '@angular/core';
import { BaseUrl } from '../../app.config';
import { HttpClient } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { Observable, catchError, map } from 'rxjs';
import { TodosService } from '../../services/todos.service';
import { PaginationModel } from '../../models/pagination-model';
import { TodoListItem } from '../../models/todos/todo-list-item';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { TodoListItemComponent } from "./todo-list-item/todo-list-item.component";

@Component({
  selector: 'app-todos',
  standalone: true,
  imports: [
    NgIf,
    NgFor,
    MatButtonModule,
    AsyncPipe,
    TodoListItemComponent
],
  templateUrl: './todos.component.html',
  styleUrl: './todos.component.css',
})
export class TodosComponent implements OnInit
{

  pagingModel?: Observable<PaginationModel<TodoListItem>>

  constructor(
    private httpClient: HttpClient,
    private todoService: TodosService) {}  

  // authTest() {
  //   this.httpClient
  //     .get(BaseUrl + '/api/todos/getuser')
  //     .subscribe((r) => console.log(r));
  // }

  ngOnInit(): void {
    this.pagingModel = this.todoService.getTodos();
  }
}
