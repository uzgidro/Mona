import { UserModel } from './../../models/user';
import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MessageService } from '../../services/message.service';
import { MessageRequest } from '../../models/message';

@Component({
  selector: 'app-send-message',
  templateUrl: './send-message.component.html',
  styleUrl: './send-message.component.css'
})
export class SendMessageComponent {
  files:FileList
  textInput:string
  @Input() selectedChat:UserModel
  constructor(private messageService:MessageService){}



  handleFileInput(event: any) {
    this.files= event.target.files;
  }
  sendMessage(){
   console.log(this.textInput);
   console.log(this.files);
   console.log(this.selectedChat.firstName);
    const messageRequest: MessageRequest = {
    text: this.textInput ? this.textInput : '',
    receiverId: this.selectedChat?.id,
    createdAt: new Date(),
  };

   this.messageService.sendMessage(messageRequest)

   this.textInput=''

  }
}
