import {Component, OnInit} from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent implements OnInit {

  ngOnInit() {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://127.0.0.1:5031/hub")
      .build();

    connection.on("ReceiveMessage", (message: any) => {
      console.log(message)
    });

    connection.start().then(() => {
      console.log('connected')
    }).catch((err) => document.write(err));

    setInterval(() => {
      connection.send("send", "Hello Abboss")
    }, 1000)
  }
}
