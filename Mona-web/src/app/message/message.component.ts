import { File } from './../models/message';
import {Component, OnInit, input} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';
import {JwtService} from "../services/jwt.service";
import { FormControl, FormGroup} from "@angular/forms";
import {UserModel} from "../models/user";
import {MessageModel, MessageRequest} from '../models/message';
import { ApiService } from '../services/api.service';

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
  })

  selectedFile?: any;
  connection?: HubConnection
  private _income: MessageModel[] = []
  editingMessage?: MessageModel
  repliedMessage?:MessageModel
  get income(): MessageModel[] {
    return this._income.filter(item => item.receiverId == this.selectedChat?.id || item.senderId == this.selectedChat?.id)
      .sort((a, b) => {
        const dateA = new Date(a.createdAt).getTime();
        const dateB = new Date(b.createdAt).getTime();
        return dateA - dateB;
      });
  }
  constructor(private jwtService: JwtService, private apiService:ApiService) {
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
    if (!this.editingMessage){
      let message = this.inputGroup.get('message')?.value;
      let replyId: string|undefined=this.repliedMessage?this.repliedMessage.id:undefined;
          if(this.selectedFile&&message){
            const messageRequest: MessageRequest = {
            text: message,
            receiverId: this.selectedChat?.id,
            createdAt: new Date(),
            replyId: replyId
            };
            const formData = new FormData();
             formData.append('message',JSON.stringify(messageRequest))
             formData.append("file", this.selectedFile, this.selectedFile.name);
             this.apiService.sendMessage(formData)
            }
            if(this.selectedFile&&message==''){
              const messageRequest: MessageRequest = {
                text:'',
                receiverId: this.selectedChat?.id,
                createdAt: new Date(),
                replyId: replyId
                };
                const formData = new FormData();
                formData.append('message',JSON.stringify(messageRequest))
                formData.append("file", this.selectedFile, this.selectedFile.name);
                this.apiService.sendMessage(formData)
            }
            if(!this.selectedFile&&message){
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
                  messagesToSend.forEach((text)=>{
                  const messageReq:MessageRequest={
                    text: text,
                    receiverId: this.selectedChat?.id,
                    createdAt: new Date(),
                    replyId: replyId
                  }
                  const formData = new FormData();
                  formData.append('message', JSON.stringify(messageReq));
                  this.apiService.sendMessage(formData);
                  })
                }
               //WITHOUT DIVIDING CHARACTER'S NUMBER AND SPLITING THEM INTO A SINGLE MESSAGE  EVEN IF THEY ARE BIGGER THAN 20
              // const messageRequest: MessageRequest = {
              //   text: message,
              //   receiverId: this.selectedChat?.id,
              //   createdAt: new Date(),
              //   replyId: replyId
              //  };
              //  console.log(messageRequest);
              //   formData.append('message',JSON.stringify(messageRequest))
              //   this.apiService.sendMessage(formData)
              //   }

               this.inputGroup.get('message')?.setValue('')
              }
              else if (this.editingMessage) {

              const inputValue = this.inputGroup.get('message')?.value||'';
            // DIVIDING CHARACTER'S NUMBER AND SPLIT THEM INTO A SINGLE MESSAGE IF THEY ARE BIGGER THAN 20
              const messagesToSend:string[]=[]
              let remainingMessage = inputValue;
              while (remainingMessage.length > 20) {
                messagesToSend.push(remainingMessage.substring(0, 20));
                remainingMessage = remainingMessage.substring(20);
               }
               if (remainingMessage.length > 0) {
                messagesToSend.push(remainingMessage);
                }

                 messagesToSend.forEach((text) => {
                 const editedMessage = { ...this.editingMessage, text };
                 console.log("Edited message:", editedMessage);
                //  this.connection?.send('editMessage', editedMessage);
                 });

              //WITHOUT DIVIDING CHARACTER'S NUMBER AND SPLITING THEM INTO A SINGLE MESSAGE  EVEN IF THEY ARE BIGGER THAN 20
              this.connection?.send('editMessage', {...this.editingMessage,text:inputValue});
              //CLEARING INPUT AND EDITINGMESSAGE AFTER EDITNG MESSAGE SUCCESSFULLY
              this.inputGroup.get('message')?.setValue('');
              this.editingMessage = undefined;

                }
  }


  downloadFile(file:File){
    this.apiService.fileDownload(file.id)
  }



  editMessage(eventMessage: MessageModel) {
    this.inputGroup.get('message')?.setValue(eventMessage.text)
    this.editingMessage =eventMessage
  }

  deleteMessageForMyself(eventMessage: MessageModel) {
      this.connection?.send("deleteMessageForMyself", eventMessage)
  }

  deleteMessageForEveryone(eventMessage: MessageModel) {
    this.connection?.send("deleteMessageForEveryone", eventMessage)
  }

  replyMessage(eventMessage:MessageModel){
    this.repliedMessage=eventMessage
  }
  getIncomingMessagesCount(user: UserModel): number {
    const userId = user.id;
    return this._income.filter(message => (message.senderId == userId )).length;
  }
  getSentMessagesCount(user: UserModel): number {
    const userId = user.id;
    return this._income.filter(message => (message.receiverId == userId )).length;
  }

  onFileSelected(event:any) {
    this.selectedFile=event.target.files[0]
  }

}


