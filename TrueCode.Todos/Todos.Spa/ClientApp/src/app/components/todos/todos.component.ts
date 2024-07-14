import { Component, Inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {
  MatSnackBar,
  MatSnackBarModule,
} from '@angular/material/snack-bar';
import { TodosService } from '../../services/todos.service';
import { PaginationModel } from '../../models/pagination-model';
import {
  DefaultListItem,
  ITodoListItem,
} from '../../models/todos/todo-list-item';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { TodoListItemComponent } from './todo-list-item/todo-list-item.component';
import {
  MatDialog,
  MatDialogModule,
} from '@angular/material/dialog';
import { TodoListItemDetailViewComponent } from './todo-list-item-detail-view/todo-list-item-detail-view.component';
import { DialogData } from '../../models/dialog-data';
import { _MatListItemGraphicBase } from '@angular/material/list';

@Component({
  selector: 'app-todos',
  standalone: true,
  imports: [
    NgIf,
    NgFor,
    AsyncPipe,
    MatButtonModule,
    MatDialogModule,
    MatSnackBarModule,
    TodoListItemComponent,
  ],
  templateUrl: './todos.component.html',
  styleUrl: './todos.component.css',
})
export class TodosComponent implements OnInit {
  pagingModel: PaginationModel<ITodoListItem> | null = null;

  constructor(
    private _snackBar: MatSnackBar,
    @Inject(MatDialog) private dialog: MatDialog,
    private todoService: TodosService
  ) {}

  ngOnInit(): void {
    this.todoService.getTodos().subscribe((r) => {
      this.pagingModel = r;
    });
  }

  get pageData(): ITodoListItem[] | null {
    return this.pagingModel?.pageData || null;
  }

  addNewTodo() {
    const dialogData: DialogData<ITodoListItem> = {
      newObject: true,
      data: Object.assign({}, DefaultListItem),
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
      this.pagingModel!.pageData = [...data];
    });
  }

  onDeleted(id: number) {
    const pageData = this.pageData!;
    for (var i = pageData.length - 1; i >= 0; i--) {
      if (pageData[i].id === id) {
        pageData.splice(i, 1);
        this._snackBar.open('Todo item removed successfuly!', 'Splash', {
          horizontalPosition: 'start',
          verticalPosition: 'top'
        })
        break;
      }
    }
    //
  }
}
