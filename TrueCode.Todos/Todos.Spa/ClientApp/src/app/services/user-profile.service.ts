import { Injectable } from '@angular/core';
import { IUserProfile } from '../models/user-models';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  private _userProfile!: Nullable<IUserProfile>;

  constructor() { }

  public setUserProfile(value: IUserProfile) {
    this._userProfile = value;
    localStorage.setItem("userProfile", JSON.stringify(value))
  }

  public clearUserProfile()
  {
    this._userProfile = null;;
  }

}

type Nullable<T> = T | null;
