import { Component, Inject, OnInit } from '@angular/core';
import { BaseUrl } from '../../app.config';
import { HttpClient } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { BehaviorSubject, Observable, catchError, map } from 'rxjs';
import { TodosService } from '../../services/todos.service';
import { PaginationModel } from '../../models/pagination-model';
import {
  DeafultListItem,
  ITodoListItem,
} from '../../models/todos/todo-list-item';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { TodoListItemComponent } from './todo-list-item/todo-list-item.component';
import {
  MatDialog,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { TodoListItemDetailViewComponent } from './todo-list-item-detail-view/todo-list-item-detail-view.component';
import { DialogData } from '../../models/dialog-data';

@Component({
  selector: 'app-todos',
  standalone: true,
  imports: [
    NgIf,
    NgFor,
    MatButtonModule,
    MatDialogModule,
    AsyncPipe,
    TodoListItemComponent,
  ],
  templateUrl: './todos.component.html',
  styleUrl: './todos.component.css',
})
export class TodosComponent implements OnInit {  
  pagingModel: PaginationModel<ITodoListItem> | null = null;  

  constructor(
    @Inject(MatDialog) private dialog: MatDialog,
    private todoService: TodosService
  ) {    
    
  }  

  ngOnInit(): void {
    
    this.todoService.getTodos().subscribe(r => {            
      this.pagingModel = r;       
    });
    
  }

  get pageData():ITodoListItem[] | null {
    return this.pagingModel?.pageData || null;
  }

  addNewTodo() {
    const dialogData: DialogData<ITodoListItem> = {
      newObject: true,
      data: DeafultListItem,
    };

    //open modal and add
    const dialogRef = this.dialog.open<
      TodoListItemDetailViewComponent,
      DialogData<ITodoListItem>,
      ITodoListItem
    >(TodoListItemDetailViewComponent, {
      width: '350px',
      data: dialogData,
    });

    dialogRef.afterClosed().subscribe((result) => {
      //cancel edit in any way
      if (!result) return;     
      
      //nah, immutability does create memory traffic
      var data = this.pagingModel!.pageData;
      data.unshift(result);
      this.pagingModel!.pageData  = [...data];
    });
  }
}
