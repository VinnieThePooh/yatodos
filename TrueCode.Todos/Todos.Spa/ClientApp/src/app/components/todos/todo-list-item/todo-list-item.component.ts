import { Component, Input } from '@angular/core';
import { ITodoListItem } from '../../../models/todos/todo-list-item';
import {
  MatCard,
  MatCardActions,
  MatCardContent,
  MatCardHeader,
  MatCardSubtitle,
  MatCardTitle,
} from '@angular/material/card';
import {
  MatButtonToggle,
  MatButtonToggleChange,
  MatButtonToggleGroup,
} from '@angular/material/button-toggle';
import { TodosService } from '../../../services/todos.service';

@Component({
  selector: 'app-todo-list-item',
  standalone: true,
  imports: [
    MatCard,
    MatCardHeader,
    MatCardTitle,
    MatCardSubtitle,
    MatCardContent,
    MatCardActions,
    MatButtonToggle,
    MatButtonToggleGroup,
  ],
  templateUrl: './todo-list-item.component.html',
  styleUrl: './todo-list-item.component.css',
})
export class TodoListItemComponent {
  @Input() item!: ITodoListItem;

  constructor(private todoService: TodosService) {}

  onChange(v: MatButtonToggleChange) {
    console.log(`Value changed: ${v.value}`);
    this.todoService.updatePriority(this.item.id, Number(v.value)).subscribe(r => console.debug(r));
  }
}
