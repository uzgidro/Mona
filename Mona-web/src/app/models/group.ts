import {UserModel} from "./user";

export interface GroupModel {
  id: string
  name: string
  description?: string
  users?: UserModel[]
}
