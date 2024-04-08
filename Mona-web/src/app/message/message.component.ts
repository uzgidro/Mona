import {Component, OnInit} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';
import {JwtService} from "../services/jwt.service";
import { FormControl, FormGroup} from "@angular/forms";
import {UserModel} from "../models/user";
import {MessageModel, MessageRequest} from '../models/message';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent implements OnInit {

  files:any[]=[]
  users: UserModel[] = []
  selectedChat?: UserModel
  inputGroup = new FormGroup({
    message: new FormControl(''),
    file: new FormControl('')

  })

  connection?: HubConnection
  private _income: MessageModel[] = []
  editingMessage?: MessageModel
  repliedMessage?:MessageModel
  get income(): MessageModel[] {
    return this._income.filter(item => item.receiverId == this.selectedChat?.id || item.senderId == this.selectedChat?.id)
      .sort((a, b) => {
        const dateA = new Date(a.createdAt).getTime();
        const dateB = new Date(b.createdAt).getTime();
        return dateA - dateB;
      });
  }
  constructor(private jwtService: JwtService) {
  }

  ngOnInit() {
    let accessToken = this.jwtService.getAccessToken()
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://127.0.0.1:5031/hub", {
        accessTokenFactory(): string | Promise<string> {
          return accessToken
        }
      })
      .build();
    this.connection = connection
    connection.on("ReceiveMessage", (message: MessageModel) => {
      this._income.push(message)
    });
    connection.on("ModifyMessage", (modifiedMessage: MessageModel) => {
      const index = this._income.findIndex(item => item.id === modifiedMessage.id);
      this._income[index] = modifiedMessage;
    });
    connection.on("DeleteMessage", (deletedMessage: MessageModel) => {
      this._income = this._income.filter(item => item.id !== deletedMessage.id);
    });
    connection.start()
      .catch((err) => {
        console.log(err)
      })
      .then(() => {
        if (connection) {
          connection.invoke('getUsers').then((users: UserModel[]) => this.users = users)
          connection.invoke('getHistory').then((messages: MessageModel[]) => {
            this._income = messages
            console.log(this._income);
          })
        }
      })
  }

  selectChat(user: UserModel) {
    this.selectedChat = user
  }
  sendMessage() {
    if (this.files) {

     console.log(this.files);
     const messageReq:MessageRequest={
      text: this.files[0].name,
      receiverId: this.selectedChat?.id,
      createdAt: new Date(),
     }
     this.connection?.send("sendDirectMessage", messageReq);
    }



    if (!this.editingMessage && this.inputGroup.get('message')?.value) {
      let message = this.inputGroup.get('message')?.value;
      let replyId: string;
      if (this.repliedMessage) {
        replyId = this.repliedMessage.id;
      }
      if (message) {
        const messagesToSend: string[] = [];
        let remainingMessage = message;
        while (remainingMessage.length > 20) {
          messagesToSend.push(remainingMessage.substring(0, 20));
          remainingMessage = remainingMessage.substring(20);
        }
        if (remainingMessage.length > 0) {
          messagesToSend.push(remainingMessage);
        }
        messagesToSend.forEach(chunk => {
          const messageRequest: MessageRequest = {
            text: chunk,
            receiverId: this.selectedChat?.id,
            createdAt: new Date(),
            replyId: replyId
          };
          this.connection?.send("sendDirectMessage", messageRequest);
        });
        this.inputGroup.get('message')?.setValue('');
        this.repliedMessage = undefined;
      }
    } else if (this.editingMessage) {
      const inputValue = this.inputGroup.get('message')?.value;
      if (inputValue) {
        this.editingMessage.text = inputValue;
        this.connection?.send("editMessage", this.editingMessage);
        this.inputGroup.get('message')?.setValue('');
        this.editingMessage = undefined;
      }
    }
  }

  editMessage(eventMessage: MessageModel) {
    this.inputGroup.get('message')?.setValue(eventMessage.text)
    this.editingMessage =eventMessage
  }

  deleteMessageForMyself(eventMessage: MessageModel) {
      this.connection?.send("deleteMessageForMyself", eventMessage)
  }

  deleteMessageForEveryone(eventMessage: MessageModel) {
    this.connection?.send("deleteMessageForEveryone", eventMessage)
  }

  replyMessage(eventMessage:MessageModel){
    this.repliedMessage=eventMessage
  }
  getIncomingMessagesCount(user: UserModel): number {
    const userId = user.id;
    return this._income.filter(message => (message.senderId == userId )).length;
  }
  getSentMessagesCount(user: UserModel): number {
    const userId = user.id;
    return this._income.filter(message => (message.receiverId == userId )).length;
  }


  onFileSelected(event: any) {
    this.files=event.target.files
    console.log(event);

  }


}


