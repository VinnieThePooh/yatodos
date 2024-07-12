export class TodoListItem {
    id?:number;
    title!:string;
    description!:string;
    priority!:number;
    isCompleted!:boolean;
    dueDate?:Date;
    createDate?:Date;
}