import { UserModel } from "./user";

export interface MessageRequest{
  text:string
  senderId?:string
  receiverId:string|number|undefined
  replyId?: string
  forwardId?: string
  createdAt:Date
}


export interface File {
  id: string;
  name: string;
  path: string;
  size: number;
  messageId: string;
  isDeleted: boolean;
  createdAt: string;
}

export interface MessageModel{
  id: string;
  text: string;
  senderId: string;
  sender: UserModel;
  directReceiverId?: string;
  groupReceiverId?: string;
  receiver: UserModel;
  replyId?: any;
  repliedMessage?: MessageModel
  isEdited: boolean;
  isPinned: boolean;
  forwardedMessage?: MessageModel;
  isDeleted: boolean;
  createdAt: string;
  modifiedAt: string;
  files:File[]
}
