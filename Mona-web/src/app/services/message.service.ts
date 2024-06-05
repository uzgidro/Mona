import { Injectable } from "@angular/core";
import { JwtService } from "./jwt.service";
import { ApiService } from "./api.service";
import { MessageModel, MessageRequest } from "../models/message";
import { HubConnection } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';
import { UserModel } from "../models/user";
import { GroupModel } from "../models/group";

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  chatConnection?: HubConnection;
  users: UserModel[];
  groups: GroupModel[] = [];
  private _income: MessageModel[];

  constructor(private apiService: ApiService, private jwtService: JwtService) {
    let accessToken = this.jwtService.getAccessToken();
    this.setChatConnection(accessToken);
  }

  get income(): MessageModel[] {
    return this._income.filter(item => item);
  }

  sendMessage(messageRequest: MessageRequest) {
    let formData = new FormData();
    formData.append('message', JSON.stringify(messageRequest));
    this.apiService.sendMessage(formData);
  }

  deleteMessageForMyself(eventMessageId: string) {
    this.chatConnection?.send("deleteMessageForMyself", eventMessageId)
      .then(() => {
        console.log("Message deleted successfully for self.");
      })
      .catch((error) => {
        console.error("Error deleting message for self:", error);
      });
  }


  deleteMessageForEveryone(eventMessageId: string) {
    this.chatConnection?.send("deleteMessageForEveryone", eventMessageId);
  }


  copyMessage(message: MessageModel): void {
    // You can implement copying logic here
    // For example, you can use the Clipboard API to copy the message content to the clipboard
    const messageContent = message.text;
    navigator.clipboard.writeText(messageContent)
      .then(() => {
        console.log('Message copied to clipboard:', messageContent);
        // Optionally, you can provide user feedback here
      })
      .catch((error) => {
        console.error('Failed to copy message to clipboard:', error);
        // Optionally, you can handle errors here
      });
  }











 setChatConnection(accessToken: string) {
    const chatConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://127.0.0.1:5031/chat", {
        accessTokenFactory(): string | Promise<string> {
          return accessToken;
        }
      })
      .build();
    this.chatConnection = chatConnection;
    chatConnection.on("ReceiveMessage", (message: MessageModel) => {
      this._income.push(message);
      console.log(this.income);
    });
    chatConnection.on("ModifyMessage", (modifiedMessage: MessageModel) => {
      const index = this._income.findIndex(item => item.id === modifiedMessage.id);
      this._income[index] = modifiedMessage;
    });
    chatConnection.on("DeleteMessage", (deletedMessageId: string) => {
      console.log("Deleted Message Id:", deletedMessageId);
      this._income = this._income.filter(item => {
        console.log("Message Id:", item.id);
        return item.id !== deletedMessageId;
      });
    });
    chatConnection.on("PinMessage", (pinnedMessage: MessageModel) => {
      console.log(pinnedMessage);
    });
    chatConnection.on("ReceiveException", (exception: any) => {
      console.log(exception);
    });
    chatConnection.start()
      .catch((err) => {
        console.log(err);
      })
      .then(() => {
        if (chatConnection) {
          chatConnection.invoke('getUsers').then((users: UserModel[]) => this.users = users);
          chatConnection.invoke('getHistory').then((messages: MessageModel[]) => {
            this._income = messages;
            console.log(this._income);
          });
        }
      });
  }
}
