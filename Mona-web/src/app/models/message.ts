import { UserModel } from "./user";

export interface MessageRequest{
  text:string
  senderId?:string
  receiverId:string|number|undefined
  createdAt:Date
}


export interface MessageModel{
  id: string;
  text: string;
  senderId: string;
  sender: UserModel;
  receiverId: string;
  receiver: UserModel;
  replyId?: any;
  isEdited: boolean;
  isForwarded: boolean;
  isDeleted: boolean;
  createdAt: string;
  modifiedAt: string;
}