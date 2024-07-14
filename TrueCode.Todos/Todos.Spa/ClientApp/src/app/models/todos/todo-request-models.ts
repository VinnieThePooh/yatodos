export interface ITodoCreateRequest {    
    title: string;
    description: string;
    priority: number;    
    isCompleted: boolean;
    dueDate?: Date;    
  }

  export interface ITodoUpdateRequest {    
    id:number;
    userId: number;
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