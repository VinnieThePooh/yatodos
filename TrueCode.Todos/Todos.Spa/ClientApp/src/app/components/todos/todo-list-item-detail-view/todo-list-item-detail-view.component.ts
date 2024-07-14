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
import { ITodoCreateRequest, ITodoUpdateRequest } from '../../../models/todos/todo-request-models';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { UserProfileService } from '../../../services/user-profile.service';

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
    private profileService: UserProfileService,
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
    if (this.data.newObject) {
      const request: ITodoCreateRequest = {
        title: this.todoItem.title,
        description: this.todoItem.description,
        priority: this.todoItem.priority,
        isCompleted: this.todoItem.isCompleted,
        dueDate: this.todoItem.dueDate,
      };

      this.todoService.createTodo(request).subscribe((r) => {
        this.todoItem.id = r.id;
        this.todoItem.createDate = r.createDate;
        this.dialogRef.close(this.todoItem);
      });
      return;
    }

    var userId = this.profileService.UserProfile!.userId;

    //update request
    const request: ITodoUpdateRequest = {
      title: this.todoItem.title,
      id: this.todoItem.id,
      userId: userId,
      description: this.todoItem.description,
      priority: this.todoItem.priority,
      isCompleted: this.todoItem.isCompleted,
      dueDate: this.todoItem.dueDate,
    };

    this.todoService.updateTodo(request).subscribe((r) => {            
      this.dialogRef.close(this.todoItem);
    });
  }
}
