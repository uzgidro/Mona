export interface ChatDto {
  chatId: string;
  chatName: string;
  message?: string;
  messageTime: Date;
  receiverId: string;
  senderId: string;
  senderName: string;
  isForward: boolean;
  chatIcon?: string;
}
