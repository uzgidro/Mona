import {File} from './../models/message';
import {Component, OnInit} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';
import {JwtService} from "../services/jwt.service";
import {FormControl, FormGroup} from "@angular/forms";
import {UserModel} from "../models/user";
import {MessageModel, MessageRequest} from '../models/message';
import {ApiService} from '../services/api.service';
import {MatDialog} from '@angular/material/dialog'
import { ForwardMessageDialogComponent } from './dialog/dialog.component';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent implements OnInit {

  users: UserModel[] = []
  selectedChat?: UserModel
  inputGroup = new FormGroup({
    message: new FormControl(''),
    file:new FormControl('')
  })

  selectedFiles?: any[];
  connection?: HubConnection
  private _income: MessageModel[] = []
  editingMessage?: MessageModel
  repliedMessage?: MessageModel
  forwardedMessage?: MessageModel

  get income(): MessageModel[] {
    return this._income.filter(item => item.receiverId == this.selectedChat?.id || item.senderId == this.selectedChat?.id)
  }

  constructor(private jwtService: JwtService, private apiService: ApiService, private dialog:MatDialog) {
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
      console.log(this.income);

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
    let message = this.inputGroup.get('message')?.value;
    let replyId: string | undefined = this.repliedMessage ? this.repliedMessage.id : undefined;
    let forwardId:string|undefined=this.forwardedMessage?this.forwardedMessage.id:undefined;
    if (this.selectedFiles?.length) {
      const messageRequest: MessageRequest = {
        text: message ? message : '',
        receiverId: this.selectedChat?.id,
        createdAt: new Date(),
        replyId: replyId,
        forwardId:forwardId
      };
      let formData = new FormData();
      formData.append('message', JSON.stringify(messageRequest))
      const filesArr = [...this.selectedFiles]
      filesArr.forEach(file => {
        formData.append("file", file, file.name);
      });
      this.apiService.sendMessage(formData)
      //CLEARING INPUT AND EDITINGMESSAGE AFTER SENDING FORMDATA TO SERVER SUCCESSFULLY
      this.inputGroup.get('file')?.setValue('')
    } else {
      //IF THERE IS NO FILE SELECTED AND ONLY MESSAGE SHOULD BE SEND
      // DIVIDING CHARACTER'S NUMBER AND SPLIT THEM INTO A SINGLE MESSAGE IF THEY ARE BIGGER THAN 20
      const messagesToSend: string[] = [];
      let remainingMessage = message;
      while (remainingMessage.length > 20) {
        messagesToSend.push(remainingMessage.substring(0, 20));
        remainingMessage = remainingMessage.substring(20);
      }
      if (remainingMessage.length > 0) {
        messagesToSend.push(remainingMessage);
      }
      messagesToSend.forEach((text) => {
        const messageReq: MessageRequest = {
          text: text,
          receiverId: this.selectedChat?.id,
          createdAt: new Date(),
          replyId: replyId,
          forwardId:forwardId
        }
        const formData = new FormData();
        formData.append('message', JSON.stringify(messageReq));
        this.apiService.sendMessage(formData);
      })
    }
    this.inputGroup.get('message')?.setValue('')
  }


  forwardMessage(eventMessage: MessageModel) {
     this.forwardedMessage = eventMessage
     console.log(this.forwardedMessage);
     let replyId:string
     if (this.repliedMessage) {
     replyId = this.repliedMessage.id;
    }
     if (this.forwardedMessage) {
      const forwardId = this.forwardedMessage.id;
      const dialogRef = this.dialog.open(ForwardMessageDialogComponent, {
        width: '400px',
        data: { forwardedMessage: this.forwardedMessage, users: this.users }
      });

      dialogRef.afterClosed().subscribe((selectedRecipients: UserModel[]) => {
        if (selectedRecipients && selectedRecipients.length > 0) {
          selectedRecipients.forEach(recipient => {
            const messageRequest: MessageRequest = {
              text:this.forwardedMessage.text,
              receiverId: recipient.id,
              createdAt: new Date(),
              replyId: replyId,
              forwardId: forwardId
            };
            this.connection?.send("sendDirectMessage", messageRequest);
          });
        }
      });

      this.forwardedMessage = undefined;
      return;
    }


  }
  editMessage() {
    const inputValue = this.inputGroup.get('message')?.value || ''
    this.connection?.send('editMessage', {...this.editingMessage, text: inputValue});
    //CLEARING INPUT AND EDITINGMESSAGE AFTER EDITNG MESSAGE SUCCESSFULLY
    this.inputGroup.get('message')?.setValue('');
    this.editingMessage = undefined;
  }

  onSelectEditingMessage(eventMessage: MessageModel) {
    this.inputGroup.get('message')?.setValue(eventMessage.text)
    this.editingMessage = eventMessage
  }


  downloadFile(file: File) {
    this.apiService.downloadFile(file)
  }

  deleteMessageForMyself(eventMessage: MessageModel) {
    this.connection?.send("deleteMessageForMyself", eventMessage)
  }

  deleteMessageForEveryone(eventMessage: MessageModel) {
    this.connection?.send("deleteMessageForEveryone", eventMessage)
  }

  replyMessage(eventMessage: MessageModel) {
    this.repliedMessage = eventMessage
  }

  getIncomingMessagesCount(user: UserModel): number {
    const userId = user.id;
    return this._income.filter(message => (message.senderId == userId)).length;
  }

  getSentMessagesCount(user: UserModel): number {
    const userId = user.id;
    return this._income.filter(message => (message.receiverId == userId)).length;
  }

  onFileSelected(event: any) {
    this.selectedFiles = event.target.files
    console.log(this.selectedFiles);
  }
}


