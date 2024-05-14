import { Injectable } from "@angular/core";
import { JwtService } from "./jwt.service";
import { ApiService } from "./api.service";
import { MessageModel, MessageRequest } from "../models/message";
import { HubConnection } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';
import { UserModel } from "../models/user";
import { GroupModel } from "../models/group";

@Injectable({
  providedIn: 'root'
})
export class MessageService{

  chatConnection?: HubConnection
  groupConnection?: HubConnection
  users: UserModel[];
  groups: GroupModel[] = []


  constructor(private apiService: ApiService) {
   }





   sendMessage(messageRequest: MessageRequest) {


      let formData = new FormData();
      formData.append('message', JSON.stringify(messageRequest))
      this.apiService.sendMessage(formData)


    }









}

