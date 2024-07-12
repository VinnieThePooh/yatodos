export class TodoListItem {
    id?:number;
    title!:string;
    description!:string;
    priority!:number;
    priorityName!:string;
    isCompleted!:boolean;
    dueDate?:Date;
    createDate?:Date;
}