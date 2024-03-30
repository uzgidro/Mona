import {Component, OnInit} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';
import {JwtService} from "../services/jwt.service";
import {FormControl, FormGroup} from "@angular/forms";
import {UserModel} from "../models/user";
import {MessageModel, MessageRequest} from '../models/message';
import {ApiService} from "../services/api.service";

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

  constructor(private jwtService: JwtService, private apiService: ApiService) {
  }

  async ngOnInit() {
    this.setupConnection()
    this.openConnection()
    await this.startConnection()
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

  deleteMessageForMyself(message: MessageModel) {
    this.connection?.send("deleteMessageForMyself", message)

  }

  deleteMessageForEveryone(message: MessageModel) {
    this.connection?.send("deleteMessageForEveryone", message)

  }

  private setupConnection() {
    let accessToken = this.jwtService.getAccessToken()
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("http://127.0.0.1:5031/hub", {
        accessTokenFactory(): string | Promise<string> {
          return accessToken
        }
      })
      .build()
  }

  private openConnection() {
    this.connection?.on("ReceiveMessage", (message: MessageModel) => {
      this._income.push(message);
    });
    this.connection?.on("ModifyMessage", (modifiedMessage: MessageModel) => {
      const index = this._income.findIndex(item => item.id === modifiedMessage.id);
      if (index !== -1) {
        modifiedMessage.isEdited = true
        this._income[index] = modifiedMessage;
      }
    });
    this.connection?.on("DeleteMessage", (deletedMessage: MessageModel) => {
      this._income = this._income.filter(item => item.id !== deletedMessage.id);
    });
  }

  private async startConnection() {
    try {
      if (this.connection) {
        await this.connection.start()
        this.connection.invoke('getUsers').then((users: UserModel[]) => this.users = users)
        this.connection.invoke('getHistory').then((messages: MessageModel[]) => {
          this._income = messages
        })
      }
    } catch (err: any) {
      if (err.toString().includes('401')) {
        await this.restartConnection()
      } else {
        console.log(err)
      }
    }
  }

  private async restartConnection() {
    await this.connection?.stop()
    await this.apiService.refreshTokens()
    this.setupConnection()
    await this.startConnection()
  }
}
