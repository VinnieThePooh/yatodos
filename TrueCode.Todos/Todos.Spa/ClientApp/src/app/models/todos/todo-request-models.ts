export interface ITodoCreateRequest {    
    title: string;
    description: string;
    priority: number;    
    isCompleted: boolean;
    dueDate?: Date;    
  }
  
  export const TodoCreateDefaultData: ITodoCreateRequest = {    
    title: '',
    description: '',
    priority: 0,    
    isCompleted: false,
    dueDate: undefined,    
  };