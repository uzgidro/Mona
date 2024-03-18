


export interface MessageRequest{
  text:string
  senderId?:string
  receiverId:string|number|undefined
  createdAt:Date
}
