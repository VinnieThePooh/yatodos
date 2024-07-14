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
import { priorityColorMap, priorityNameMap } from '../../../models/priority-maps';
import { NgStyle } from '@angular/common';

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
    NgStyle,
    MatButtonToggleGroup,
  ],
  templateUrl: './todo-list-item.component.html',
  styleUrl: './todo-list-item.component.css',
})
export class TodoListItemComponent {
  @Input() item!: ITodoListItem;

  constructor(private todoService: TodosService) {}

  onChange(v: MatButtonToggleChange) {        

    console.log(`Priority: ${this.item.priority}`);
    const newValue = Number(v.value);

    
    this.todoService.updatePriority(this.item.id, newValue).subscribe(r => {      
      this.item.priority = newValue;
    });
  }

  isChecked(elementRef:MatButtonToggle): boolean {
    return elementRef.value == this.item.priority;
  }

  get priorityName(): string {
    return priorityNameMap[this.item.priority];
  }

  get priorityColor(): string {
    return priorityColorMap[this.item.priority];
  }

  get priorityColorMap(): string[] {
    return priorityColorMap;
  }
}
