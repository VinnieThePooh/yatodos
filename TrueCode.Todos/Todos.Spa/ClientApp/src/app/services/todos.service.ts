import { Injectable } from '@angular/core';
import { IUserProfile } from '../models/user-models';
import { Nullable, UserProfileService } from './user-profile.service';
import { Observable } from 'rxjs';
import { ITodoCreateRequest, ITodoUpdateRequest } from '../models/todos/todo-request-models';
import { ITodoListItem } from '../models/todos/todo-list-item';
import { ApiUrls } from '../app.config';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PaginationModel } from '../models/pagination-model';
import { ITodoCreateResponse } from '../models/todos/todo-response-models';

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

  createTodo(request: ITodoCreateRequest): Observable<ITodoCreateResponse> {
    console.log(JSON.stringify(request));
    return this.httpClient.post<ITodoCreateResponse>(this.targetUrl, request);
  }

  //todo: return type?
  updateTodo(listItem: ITodoUpdateRequest): Observable<object> {    
    return this.httpClient.put(this.targetUrl, listItem);
  }

  updatePriority(todoId:number, newPriority:number): Observable<object> {

    var userId = this.profileService.UserProfile!.userId;
    var url = `${this.targetUrl}/priority`;

    const request = {
      userId: userId,
      priority: newPriority,
      todoId: todoId
    };

    return this.httpClient.put(url, request);
  }

  deleteTodo(id:number): Observable<object> {    
    return this.httpClient.delete(this.targetUrl + "/" + id);
  }
}

export const Defaults = {
  PAGE_NUMBER: 1,
  PAGE_SIZE: 10,
};
