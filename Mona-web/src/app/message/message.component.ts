import { ForwardMessageComponent } from './message-actions/forward-message/forward-message.component';
import { UserModel } from './../models/user';
import {File, MessageModel, MessageRequest} from '../models/message';
import {Component, OnInit, ViewChild} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';
import {JwtService} from "../services/jwt.service";
import {FormControl, FormGroup} from "@angular/forms";
import {ApiService} from '../services/api.service';
import {MatDialog} from '@angular/material/dialog'
import {GroupModel} from "../models/group";
import { MatSidenav } from '@angular/material/sidenav';
import { MessageActionsComponent } from './message-actions/message-actions.component';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent implements OnInit {
  @ViewChild('sidenav') sidenav!: MatSidenav;

  toggleSidenav() {
    this.sidenav.toggle();
  }

  users: UserModel[] = []
  groups: GroupModel[] = []
  selectedChat?: any
  inputGroup = new FormGroup({
    message: new FormControl(''),
    file: new FormControl('')
  })

  selectedFiles?:any[]
  chatConnection?: HubConnection
  groupConnection?: HubConnection
  private _income: MessageModel[] = []
  editingMessage?: MessageModel
  repliedMessage?: MessageModel
  forwardedMessage?: MessageModel
  currentUser:UserModel

  get income(): MessageModel[] {
    return this._income.filter(item => item.directReceiverId == this.selectedChat?.id || item.senderId == this.selectedChat?.id || item.groupReceiverId == this.selectedChat?.id)
  }

  constructor(private jwtService: JwtService, private apiService: ApiService, private dialog: MatDialog) {
  }

  ngOnInit() {
    let accessToken = this.jwtService.getAccessToken()
    this.setChatConnection(accessToken)
    this.setGroupConnection(accessToken)
    let id=this.jwtService.getIdFromJwt()
    this.apiService.getUserInfo(id).subscribe({
      next: (value: UserModel) => {
        this.currentUser = value; // Assigning value to currentUser
        console.log(this.currentUser);

      },
      error: (error) => {
        console.error("Error occurred while fetching user info:", error);
      }
    });


  }

  selectChat(chat: any) {
    this.selectedChat = chat
  }

  sendMessage() {
    let message = this.inputGroup.get('message')?.value;
    let replyId: string | undefined = this.repliedMessage ? this.repliedMessage.id : undefined;
    let forwardId: string | undefined = this.forwardedMessage ? this.forwardedMessage.id : undefined;
    const messageRequest: MessageRequest = {
      text: message ? message : '',
      receiverId: this.selectedChat?.id,
      createdAt: new Date(),
      replyId: replyId,
      forwardId: forwardId
    };
    if (this.selectedFiles) {
      console.log(this.selectedFiles);

      let formData = new FormData();
      formData.append('message', JSON.stringify(messageRequest))
      console.log(this.selectedFiles);

      const filesArr = [...this.selectedFiles]
      console.log(filesArr);

      filesArr.forEach(file => {
        console.log(file);

        formData.append("file", file, file.name);
      });


      this.apiService.sendMessage(formData)
      this.inputGroup.get('file')?.setValue('')
    } else {
      this.chatConnection.send("sendMessage", messageRequest)
    }
    this.inputGroup.get('message')?.setValue('')
    this.repliedMessage=undefined
  }

  forwardMessage(eventMessage: MessageModel) {
    this.forwardedMessage = eventMessage;
    if (this.forwardedMessage) {
      const dialogRef = this.dialog.open(ForwardMessageComponent, {
        width: '400px',
        data: {forwardedMessage: this.forwardedMessage, users: this.users}
      });
      dialogRef.afterClosed().subscribe(() => {
      });
    }
  }

  editMessage() {
    const inputValue = this.inputGroup.get('message')?.value || ''
    this.chatConnection?.send('editMessage', {...this.editingMessage, text: inputValue});
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

  deleteMessageForMyself(eventMessageId: string) {
    this.chatConnection?.send("deleteMessageForMyself", eventMessageId)
  }

  deleteMessageForEveryone(eventMessageId: string) {
    this.chatConnection?.send("deleteMessageForEveryone", eventMessageId)
  }

  replyMessage(eventMessage: MessageModel) {
    this.repliedMessage = eventMessage
  }

  getIncomingMessagesCount(chat: any): number {
    const chatId = chat.id;
    return this._income.filter(message => (message.senderId == chatId)).length;
  }

  getSentMessagesCount(chat: any): number {
    const chatId = chat.id;
    return this._income.filter(message => message.directReceiverId == chatId || message.groupReceiverId == chatId).length;
  }

  onFileSelected(event: any) {
    console.log(event.target.files.length);
    this.selectedFiles=event.target.files
    console.log(this.selectedFiles);
  }

  pinMessage(message:MessageModel){
    this.chatConnection?.send('PinMessage',message)
  }

  openMessageActions(message:MessageModel){
    const dialogRef = this.dialog.open(MessageActionsComponent, {
      width: '400px',
      data:
       {
        message:message,
        forwardedMessage: this.forwardedMessage,
        users: this.users,
        deleteMessageForMyself: this.deleteMessageForMyself.bind(this),
        deleteMessageForEveryone: this.deleteMessageForEveryone.bind(this),
        editMessage: this.onSelectEditingMessage.bind(this),
        currentUser:this.currentUser,
        replyMessage:this.replyMessage.bind(this),
        pinMessage:this.pinMessage.bind(this),
        },

    });
    dialogRef.afterClosed().subscribe(() => {

    });

  }






  private setChatConnection(accessToken: string) {
    const chatConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://127.0.0.1:5031/chat", {
        accessTokenFactory(): string | Promise<string> {
          return accessToken
        }
      })
      .build();
    this.chatConnection = chatConnection
    chatConnection.on("ReceiveMessage", (message: MessageModel) => {
      this._income.push(message)
      console.log(this.income);
    });
    chatConnection.on("ModifyMessage", (modifiedMessage: MessageModel) => {
      const index = this._income.findIndex(item => item.id === modifiedMessage.id);
      this._income[index] = modifiedMessage;
    });
    chatConnection.on("DeleteMessage", (deletedMessageId: string) => {
      this._income = this._income.filter(item => item.id !== deletedMessageId);
    });
    chatConnection.on("PinMessage", (pinnedMessage: MessageModel) => {
      console.log(pinnedMessage)
    });
    chatConnection.on("ReceiveException", (exception: any) => {
      console.log(exception)
    });
    chatConnection.start()
      .catch((err) => {
        console.log(err)
      })
      .then(() => {
        if (chatConnection) {
          chatConnection.invoke('getUsers').then((users: UserModel[]) => this.users = users)
          chatConnection.invoke('getHistory').then((messages: MessageModel[]) => {
            this._income = messages
            console.log(this._income);
          })
        }
      })
  }

  private setGroupConnection(accessToken: string) {
    const groupConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://127.0.0.1:5031/group", {
        accessTokenFactory(): string | Promise<string> {
          return accessToken
        }
      })
      .build();
    this.groupConnection = groupConnection
    groupConnection.on("EditGroup", (group: GroupModel) => {
      // TODO()
    });
    groupConnection.on("AppendMember", (group: GroupModel) => {
      // TODO()
    });
    groupConnection.on("RemoveMember", (group: GroupModel) => {
      // TODO()
    });
    groupConnection.on("ReceiveException", (exception: any) => {
      console.log(exception)
    });
    groupConnection.start()
      .catch((err) => {
        console.log(err)
      })
      .then(() => {
        if (groupConnection) {
          groupConnection.invoke('getUserGroupList').then((groups: GroupModel[]) => this.groups = groups)

        }
      })
  }
}


