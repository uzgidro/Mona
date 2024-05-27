import {UserModel} from "./user";

export interface GroupModel {
  id: string
  name: string
  description?: string
  users?: UserModel[]
}



export interface GroupRequest{
  name:string
  description:string,
  members?:string[]
}
