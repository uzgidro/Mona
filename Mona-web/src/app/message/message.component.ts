import {Component, OnInit} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';
import {JwtService} from "../services/jwt.service";
import {FormControl, FormGroup} from "@angular/forms";
import {UserModel} from "../models/user";
import { MessageModel, MessageRequest } from '../models/message';
import { __values } from 'tslib';

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
  private _income: MessageModel[]=[]
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

    connection.start()
      .catch((err) => {
        console.log(err)
      })
      .then(() => {
        if (connection) {
          connection.invoke('getUsers').then((users: UserModel[]) => this.users=users)
          connection.invoke('getHistory').then((messages: MessageModel[]) => {this._income=messages
          console.log(this._income);
          
          })

        }
      })


    // setInterval(() => {
    //   connection.send("send", "Hello Abboss")
    //   console.log('seent')
    // }, 5000)

    
  }

  selectChat(user: UserModel) {
    this.selectedChat = user
    console.log(this.selectedChat);
    
  }

  sendMessage() {
    let message = this.inputGroup.get('message')?.value
    if (message) {
      const messageRequest:MessageRequest={
        text: message,
        receiverId: this.selectedChat?.id,
        createdAt: new Date()
      }
      if (message.trim().length > 0) {
        this.connection?.send("sendDirectMessage", messageRequest)
        this.inputGroup.get('message')?.setValue('')
      }
    }

  }

  editMessage(message:MessageModel){
    this.inputGroup.value.message=message.text
  }
}




