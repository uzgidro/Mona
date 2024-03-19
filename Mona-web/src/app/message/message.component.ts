import {Component, OnInit} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';
import {JwtService} from "../services/jwt.service";
import {FormControl, FormGroup} from "@angular/forms";
import {UserDto} from "../models/user";
import { MessageRequest } from '../models/message';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent implements OnInit {
  users: UserDto[] = []
  selectedChat?: UserDto
  inputGroup = new FormGroup({
    message: new FormControl('')
  })
  connection?: HubConnection
  income: { sender: string, message: string }[] = []


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

    connection.on("ReceiveMessage", (message: any) => {
      console.log(message)
    });

    connection.start()
      .catch((err) => {
        console.log(err)
      })
      .then(() => {
        if (connection)
          connection.invoke('getUsers').then((users: any) => this.users=users)
          connection.invoke('getHistory').then((messages: any) => console.log(messages))
      })


    // setInterval(() => {
    //   connection.send("send", "Hello Abboss")
    //   console.log('seent')
    // }, 5000)
  }

  selectChat(user: UserDto) {
    this.selectedChat = user
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
}




