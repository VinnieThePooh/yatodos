import { Component, Input } from '@angular/core';
import { ITodoListItem } from '../../../models/todos/todo-list-item';
import { MatCard, MatCardActions, MatCardContent, MatCardHeader, MatCardSubtitle, MatCardTitle } from '@angular/material/card';

@Component({
  selector: 'app-todo-list-item',
  standalone: true,
  imports: [MatCard, MatCardHeader, MatCardTitle, MatCardSubtitle, MatCardContent, MatCardActions],
  templateUrl: './todo-list-item.component.html',
  styleUrl: './todo-list-item.component.css'
})
export class TodoListItemComponent {
  @Input() item!: ITodoListItem;
}
