import {Component, OnInit} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';
import {JwtService} from "../services/jwt.service";
import {FormControl, FormGroup} from "@angular/forms";
import {UserModel} from "../models/user";
import {MessageModel, MessageRequest} from '../models/message';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent implements OnInit {
  users: UserModel[] = []
  selectedChat?: UserModel

  inputGroup = new FormGroup({
    message: new FormControl('')
  })
  connection?: HubConnection
  private _income: MessageModel[] = []
  editingMessage?: MessageModel;

  get income(): MessageModel[] {
    return this._income.filter(item => item.receiverId == this.selectedChat?.id || item.senderId == this.selectedChat?.id);
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
      if (index !== -1) {
        modifiedMessage.isEdited = true
        this._income[index] = modifiedMessage;
      }
    });
    connection.on("DeleteMessage", (deletedMessage: MessageModel) => {
      console.log('invoked');
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
    if (this.inputGroup.get('message')?.value !== '' && this.editingMessage === undefined) {
      let message = this.inputGroup.get('message')?.value
      if (message) {
        const messageRequest: MessageRequest = {
          text: message,
          receiverId: this.selectedChat?.id,
          createdAt: new Date()
        }
        if (message.trim().length > 0) {
          this.connection?.send("sendDirectMessage", messageRequest)
          this.inputGroup.get('message')?.setValue('')
        }
      }

    } else {
      if (this.editingMessage) {
        const inputValue = this.inputGroup.get('message')?.value;
        if (inputValue !== null && inputValue !== undefined) {
          this.editingMessage.text = inputValue
          this.connection?.send("editMessage", this.editingMessage)
        }
      }
    }
  }


  editMessage(message: MessageModel) {
    this.inputGroup.get('message')?.setValue(message.text)
    this.editingMessage = message
  }

  deleteMessage(message: MessageModel) {
    if (this.selectedChat?.id === message.receiver.id) {
      this.connection?.send("deleteMessageForMyself", message)
    } else {
      console.log(message.id);
      this.connection?.send("deleteMessageForEveryone", message)
    }
  }

}


