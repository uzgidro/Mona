import { UserModel } from "./user";

export interface MessageRequest{
  text:string
  senderId?:string
  receiverId:string|number|undefined
  replyId?: string
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
  repliedMessage?: MessageModel
  isEdited: boolean;
  isForwarded: boolean;
  isDeleted: boolean;
  createdAt: string;
  modifiedAt: string;
}
