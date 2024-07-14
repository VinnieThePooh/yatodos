import { Component, Inject, Input } from '@angular/core';
import { ITodoListItem } from '../../../models/todos/todo-list-item';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOption } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';
import { MatCheckbox } from '@angular/material/checkbox';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { DialogData } from '../../../models/dialog-data';
import { TodosService } from '../../../services/todos.service';
import { MatInputModule } from '@angular/material/input';
import { ITodoCreateRequest } from '../../../models/todos/todo-request-models';
import { MatDatepickerModule } from '@angular/material/datepicker';

@Component({
  selector: 'app-todo-list-item-detail-view',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCheckbox,
    MatSelect,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatDialogModule,
    MatOption,
  ],
  templateUrl: './todo-list-item-detail-view.component.html',
  styleUrl: './todo-list-item-detail-view.component.css',
})
export class TodoListItemDetailViewComponent {

  public todoItem!: ITodoListItem;

  constructor(    
    private todoService: TodosService,
    public dialogRef: MatDialogRef<TodoListItemDetailViewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData<ITodoListItem>
  ) {
    this.todoItem = data.data;
  }

  get dialogTitle(): string {
    return this.data.newObject ? 'New todo' : 'Edit todo';
  }

  get submitTitle(): string {
    return this.data.newObject ? 'Create' : 'Update';
  }

  onCancelClick() {
    this.dialogRef.close();
  }

  onSubmitClick() {    

    const request:ITodoCreateRequest = {
      title: this.todoItem.title,
      description: this.todoItem.description,
      priority: this.todoItem.priority,
      isCompleted: this.todoItem.isCompleted,
      dueDate: this.todoItem.dueDate
    };

    this.todoService.createTodo(request).subscribe((r) => {
      this.todoItem.id = r;
      this.dialogRef.close(this.todoItem);
    });
    }  
}
