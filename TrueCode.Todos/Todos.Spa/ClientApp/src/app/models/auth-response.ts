import { IUserProfile } from "./user-models";

export interface IAuthResponse {
    token:string;     
    profile:IUserProfile;
}
