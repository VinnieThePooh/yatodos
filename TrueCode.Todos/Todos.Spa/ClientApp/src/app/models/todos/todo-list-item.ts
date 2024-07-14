export interface ITodoListItem {
  id: number;
  title: string;
  description: string;
  priority: number;
  priorityName: string;
  isCompleted: boolean;
  dueDate?: Date;
  createDate?: Date;
}

export const DefaultListItem: ITodoListItem = {
  id: 0,
  title: '',
  description: '',
  priority: 0,
  priorityName: '',
  isCompleted: false,
  dueDate: undefined,
  createDate: undefined,
};
