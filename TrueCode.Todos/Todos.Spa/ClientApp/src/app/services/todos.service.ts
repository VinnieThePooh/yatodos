import { Injectable } from '@angular/core';
import { IUserProfile } from '../models/user-models';
import { Nullable, UserProfileService } from './user-profile.service';
import { Observable } from 'rxjs';
import { ITodoCreateRequest } from '../models/todos/todo-request-models';
import { ITodoListItem } from '../models/todos/todo-list-item';
import { ApiUrls } from '../app.config';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { PaginationModel } from '../models/pagination-model';

@Injectable({
  providedIn: 'root',
})
export class TodosService {
  userProfile: Nullable<IUserProfile>;
  targetUrl: string = ApiUrls.Todos;

  constructor(
    private profileService: UserProfileService,
    private httpClient: HttpClient
  ) {
    this.userProfile = profileService.UserProfile!;
  }

  getTodos(
    pageNumber?: number,
    pageSize?: number
  ): Observable<PaginationModel<ITodoListItem>> {
    let parameters = new HttpParams();
    parameters.append('pageNumber', pageNumber ?? Defaults.PAGE_NUMBER);
    parameters.append('pageSize', pageSize ?? Defaults.PAGE_SIZE);
    parameters.append('userId', this.userProfile!.userId);
    return this.httpClient.get<PaginationModel<ITodoListItem>>(this.targetUrl, {
      params: parameters,
    });
  }

  createTodo(request: ITodoCreateRequest): Observable<number> {
    console.log(JSON.stringify(request));
    return this.httpClient.post<number>(this.targetUrl, request);
  }

  //todo: return type?
  updateTodo(listItem: ITodoListItem): Observable<object> {    
    return this.httpClient.put(this.targetUrl, listItem);
  }

  deleteTodo(id:number): Observable<object> {    
    return this.httpClient.delete(this.targetUrl + "/" + id);
  }
}

export const Defaults = {
  PAGE_NUMBER: 1,
  PAGE_SIZE: 10,
};
