import {Component, OnInit} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {JwtService} from "../services/jwt.service";

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent implements OnInit {

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

    connection.on("ReceiveMessage", (message: any) => {
      console.log(message)
    });

    connection.start().catch((err) => {
      console.log(err)
    })

    // setInterval(() => {
    //   connection.send("send", "Hello Abboss")
    //   console.log('seent')
    // }, 5000)
  }
}
