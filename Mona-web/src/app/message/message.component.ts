import {Component, OnInit} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';
import {JwtService} from "../services/jwt.service";
import {FormControl, FormGroup} from "@angular/forms";

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent implements OnInit {
  users: UserDto[] = []
  selectedChat: string = ''
  message = new FormGroup({
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

    connection.on("ReceiveMessage", (sender: string, message: string) => {
      this.income.push({sender, message})
    });

    connection.start()
      .catch((err) => {
        console.log(err)
      })
      .then(() => {
        if (connection)
          connection.invoke('getUsers').then((users: UserDto[]) => this.users = users)
      })


    // setInterval(() => {
    //   connection.send("send", "Hello Abboss")
    //   console.log('seent')
    // }, 5000)
  }

  selectChat(username: string) {
    this.selectedChat = username
  }

  sendMessage() {
    let message = this.message.get('message')?.value
    if (message) {
      if (message.trim().length > 0) {
        this.connection?.send("sendDirectMessage", this.selectedChat, message.trim())
        this.message.get('message')?.setValue('')
      }
    }

  }
}

interface UserDto {
  username: string
  firstName: string
  lastName: string
}
