import {Component, OnInit} from '@angular/core';
import {ApiService} from "../services/api.service";

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent implements OnInit {

  constructor(private api: ApiService) {
  }
  ngOnInit() {
    // const connection = new signalR.HubConnectionBuilder()
    //   .withUrl("http://127.0.0.1:5031/hub")
    //   .build();
    //
    // connection.on("ReceiveMessage", (message: any) => {
    //   console.log(message)
    // });
    //
    // connection.start().then(() => {
    //   console.log('connected')
    // }).catch((err) => document.write(err));
    //
    // setInterval(() => {
    //   connection.send("send", "Hello Abboss")
    // }, 1000)
  }

  refresh() {
    this.api.refreshTokens()
    // console.log(this.api.getTokens())
    return this.api.authCheck().subscribe({
      next: value => console.log(value)
    })
  }
}
