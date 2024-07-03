import {GroupModel, GroupRequest} from '../models/group';
import {ForwardMessageComponent} from './message-actions/forward-message/forward-message.component';
import {GetUserResponse, UserModel} from '../models/user';
import {File, FileDto, MessageDto, MessageModel, MessageRequest} from '../models/message';
import {Component, OnInit, ViewChild} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';
import {JwtService} from "../services/jwt.service";
import {FormControl, FormGroup} from "@angular/forms";
import {ApiService} from '../services/api.service';
import {MatDialog} from '@angular/material/dialog'
import {MatDrawer, MatSidenav} from '@angular/material/sidenav';
import {MessageActionsComponent} from './message-actions/message-actions.component';
import {ContactsComponent} from './contacts/contacts.component';
import {NewGroupComponent} from './new-group/new-group.component';
import {GroupActionsComponent} from './group-actions/group-actions.component';
import {DeleteGroupComponent} from './delete-group/delete-group.component';
import {ViewGroupInfoComponent} from './view-group-info/view-group-info.component';
import {HubMethods} from "../models/hub-methods";
import {HubListeners} from "../models/hub-listeners";
import { ChatDto } from '../models/chat';


@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent implements OnInit {
  @ViewChild('sidenav') sidenav!: MatSidenav;
  @ViewChild('drawer') drawer: MatDrawer;
  message: MessageModel;

  toggleSidenav() {
    this.sidenav.toggle();
  }


  checkAChat:boolean=false
  checkAGroup:boolean=false
  users: GetUserResponse[] = []
  groups: GroupModel[] = []
  selectedChat?: ChatDto
  selectedGroup?: GroupModel
  inputGroup = new FormGroup({
    message: new FormControl(''),
    file: new FormControl(''),
    groupMessage: new FormControl(''),
    groupFile: new FormControl('')
  })


  messages:MessageDto[]=[]


  chats:ChatDto[]=[]

  selectedContact?:GetUserResponse

  selectedFiles?: any[]
  chatConnection?: HubConnection
  groupConnection?: HubConnection
  private _income: MessageModel[] = []
  editingMessage?: MessageModel
  repliedMessage?: MessageModel
  forwardedMessage?: MessageModel
  currentUser: UserModel
  currentDate: Date = new Date();

  get income(): MessageModel[] {
    return this._income.filter(item => item)
  }


  constructor(private jwtService: JwtService, private apiService: ApiService, private dialog: MatDialog) {

    this.inputGroup = new FormGroup({
      message: new FormControl(''),
      file: new FormControl(''),
      groupMessage: new FormControl(''),
      groupFile: new FormControl('')
    })


  }


  ngOnInit() {
    let accessToken = this.jwtService.getAccessToken()
    this.setChatConnection(accessToken)
    this.setGroupConnection(accessToken)
    let id = this.jwtService.getIdFromJwt()
    this.apiService.getUserInfo(id).subscribe({
      next: (value: UserModel) => {
        this.currentUser = value; // Assigning value to currentUser

      },
      error: (error) => {
        console.error("Error occurred while fetching user info:", error);
      }
    });


  }

  // TODO()-3: On Chat select
  selectChat(chat: ChatDto) {
    this.messages=[]
    this.selectedContact=undefined
    this.selectedChat = chat
    this.checkAChat=true;
    this.checkAGroup=false
    this.selectedGroup = undefined
    console.log(this.selectedChat.chatId);
    this.updateMessages()
  }
  selectContact(chat:GetUserResponse) {
     this.selectedChat=undefined
     this.messages=[]
    this.selectedContact=chat
    console.log(this.selectedContact);
    if(this.selectedContact) {
      this.checkAChat=true
    }

  }
  //
  selectGroup(group: GroupModel) {
    this.selectedGroup = group
    this.selectedChat = undefined
  }

  // TODO()-4: If new chat - use receiverId, else use chatId
  sendMessage() {
    let message = this.inputGroup.get('message')?.value;
    // TODO()-5: use chatId in messageRequest
    console.log(message);

    const messageRequest: MessageRequest = {
      text:message?message:'',
      receiverId:this.selectedChat?this.selectedChat.receiverId:this.selectedContact.id,
      createdAt: new Date(),
    };
    console.log('Message request: '+messageRequest);

    if (this.selectedFiles) {
      let formData = new FormData();
      formData.append('message', JSON.stringify(messageRequest))
      const filesArr = [...this.selectedFiles]
      console.log(filesArr);

      filesArr.forEach(file => {
        console.log(file);
        formData.append("file", file, file.name);
      });

      this.apiService.sendMessage(formData)
      this.updateMessages()
      this.inputGroup.get('file')?.setValue('')
    } else {
      this.chatConnection.send(HubMethods.SendMessage, messageRequest)
      console.log('Message request: '+messageRequest);

    }
    this.updateMessages()
    this.inputGroup.get('message')?.setValue('')
    this.repliedMessage = undefined
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
    this.chatConnection?.send(HubMethods.EditMessage, {...this.editingMessage, text: inputValue});
    //CLEARING INPUT AND EDITINGMESSAGE AFTER EDITNG MESSAGE SUCCESSFULLY
    this.inputGroup.get('message')?.setValue('');
    this.editingMessage = undefined;
  }

  onSelectEditingMessage(eventMessage: MessageModel) {
    this.inputGroup.get('message')?.setValue(eventMessage.message)
    this.editingMessage = eventMessage
  }

  downloadFile(file: FileDto) {
    console.log(file);

    this.apiService.downloadFile(file)
  }

  downloadFiles(files: FileDto[]) {
    console.log(files);
    this.apiService.downloadFiles(files)
  }

  deleteMessageForMyself(eventMessage: MessageDto) {
    console.log(eventMessage);
    this.chatConnection?.send(HubMethods.DeleteMessageForMyself, eventMessage.id)
    // this.updateMessages()
  }

  deleteMessageForEveryone(eventMessage: MessageDto) {
    console.log(eventMessage);
    this.chatConnection.invoke(HubMethods.DeleteMessageForEveryone, eventMessage.id)
    // this.updateMessages()
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
    this.selectedFiles = event.target.files
    console.log(this.selectedFiles);
  }

  pinMessage(message: MessageModel) {
    this.chatConnection?.send(HubMethods.PinMessage, message)
  }


  openDeleteGroup() {
    const dialogRef = this.dialog.open(DeleteGroupComponent, {
      width: '400px',
      data:
        {
          group: this.selectedGroup,
          deleteGroups: this.deleteGroup.bind(this),
          leaveGroups: this.leaveGroup.bind(this),
        },
    });
    dialogRef.afterClosed().subscribe(() => {
    });
  }


  openMessageActions(message: MessageDto) {
    const dialogRef = this.dialog.open(MessageActionsComponent, {
      width: '400px',
      data:
        {
          message: message,
          forwardedMessage: this.forwardedMessage,
          users: this.users,
          deleteMessageForMyself: this.deleteMessageForMyself.bind(this),
          deleteMessageForEveryone: this.deleteMessageForEveryone.bind(this),
          editMessage: this.onSelectEditingMessage.bind(this),
          currentUser: this.currentUser,
          replyMessage: this.replyMessage.bind(this),
          pinMessage: this.pinMessage.bind(this),
        },

    });
    dialogRef.afterClosed().subscribe(() => {


    });


  }

  openGroupActions() {
    const dialogRef = this.dialog.open(GroupActionsComponent, {
      width: '400px',
      data:
        {
          group: this.selectedGroup,

          deleteGroups: this.deleteGroup.bind(this),
        },

    });
    dialogRef.afterClosed().subscribe(() => {


    });


  }


  openDrawer() {
    this.drawer.open()
  }

  onDrawerClosed() {
    this.dialog.open(ContactsComponent);

  }

  openContacts() {
    this.drawer.close()
    this.dialog.open(ContactsComponent, {
      width: '400px',
      data:
        {
          users: this.users,
          // TODO()-6.1: Think something
          selectContact: this.selectContact.bind(this),
        },

    });
  }

  createGroups(groupRequest: GroupRequest) {
    console.log(groupRequest);
    this.groupConnection?.send(HubMethods.CreateGroup, groupRequest)
    console.log(this.groups);

  }


  addMemberstoGroup(groupId: string, users: string[]) {
    this.groupConnection?.send(HubMethods.AppendMembers, groupId, users)
  }


  newGroup() {
    this.drawer.close()
    this.dialog.open(NewGroupComponent, {
      width: '400px',
      data:
        {
          users: this.users,
          createGroup: this.createGroups.bind(this),
          addMemberstoGroup: this.addMemberstoGroup.bind(this)
        },

    });
  }

  viewGroupInfo() {
    const dialogRef = this.dialog.open(ViewGroupInfoComponent, {
      width: '400px',
      data:
        {
          group: this.selectedGroup,
          deleteGroups: this.deleteGroup.bind(this),
          leaveGroups: this.leaveGroup.bind(this),
          // TODO()-6.2: Think something
          // selectChat: this.selectChat.bind(this)
        },
    });
    dialogRef.afterClosed().subscribe(() => {
    });

  }

  deleteGroup(group: GroupModel) {
    console.log(group);
    this.groupConnection?.send(HubMethods.DeleteGroup, group.id)
    this.groups = this.groups.filter(g => g.id !== group.id)
    this.selectedGroup = undefined
  }


  leaveGroup(group: GroupModel) {
    this.groupConnection?.send(HubMethods.LeaveGroup, group.id)
    this.groups = this.groups.filter(g => g.id !== group.id)
    this.selectedGroup = undefined
  }




  updateMessages(){
    if (this.selectedChat&&!this.selectedContact) {
      this.chatConnection.invoke(HubMethods.GetMessagesByChatId,this.selectedChat.chatId).then((m)=>{
        this.messages=m
      })

    }else if (!this.selectedChat&&this.selectedContact){
      this.chatConnection.invoke(HubMethods.GetMessagesByUserId,this.selectedContact.id).then((m)=>{
        this.messages=m
      })

    }
    
    this.chatConnection.invoke(HubMethods.GetChats).then((chats) => {
      this.chats=chats
    })




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
    chatConnection.on(HubListeners.ReceiveMessage, (message: MessageDto) => {
     console.log(message);
    });
    chatConnection.on(HubListeners.ModifyMessage, (modifiedMessage: MessageModel) => {
      const index = this._income.findIndex(item => item.id === modifiedMessage.id);
      this._income[index] = modifiedMessage;
    });
    chatConnection.on(HubListeners.DeleteMessage, (deletedMessageId: string) => {
      this.messages = this.messages.filter(item => item.id!== deletedMessageId);
    });
    chatConnection.on(HubListeners.PinMessage, (pinnedMessage: MessageModel) => {
      console.log(pinnedMessage)
    });
    chatConnection.on(HubListeners.ReceiveException, (exception: any) => {
      console.log(exception)
    });
    chatConnection.start()
      .catch((err) => {
        console.log(err)
      })
      .then(() => {
        if (chatConnection) {
          // TODO()-1: Create new model(Chat), implement, store and display
            chatConnection.invoke(HubMethods.GetChats).then((chats) => {
              console.log(chats)
              this.chats=chats
              console.log(chats);
            })
          chatConnection.invoke('getUsers').then((users: GetUserResponse[]) => {
            console.log(users)
            this.users=users
            console.log(this.users);
          }
          )
          // chatConnection.invoke('getHistory').then((messages: MessageModel[]) => {
          //   this._income = messages
          //   console.log(this._income);
          // })
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
    groupConnection.on(HubListeners.EditGroup, (updatedGroup: GroupModel) => {
      // Update the existing group in this.groups
      const groupIndex = this.groups.findIndex(group => group.id === updatedGroup.id);
      if (groupIndex !== -1) {
        this.groups[groupIndex] = updatedGroup;
        console.log("Group updated:", updatedGroup);
      }
    });
    groupConnection.on(HubListeners.AppendMember, (newGroup: GroupModel) => {
      // Add the new group to this.groups if it doesn't already exist
      if (!this.groups.some(group => group.id === newGroup.id)) {
        this.groups.push(newGroup);
        console.log("New group added:", newGroup);
      }
    });
    groupConnection.on(HubListeners.RemoveMember, (groupWithRemovedMember: GroupModel) => {
      // Update the group to remove the member
      const groupIndex = this.groups.findIndex(group => group.id === groupWithRemovedMember.id);
      if (groupIndex !== -1) {
        this.groups[groupIndex] = groupWithRemovedMember;
        console.log("Member removed from group:", groupWithRemovedMember);
      }
    });
    groupConnection.on(HubListeners.ReceiveException, (exception: any) => {
      console.log(exception)
    });
    groupConnection.start()
      .catch((err) => {
        console.log(err)
      })
    // TODO(): Not necessary, may be removed
    // .then(() => {
    //   if (groupConnection) {
    //     groupConnection.invoke('getUserGroupList').then((groups: GroupModel[]) => {
    //       this.groups = groups
    //       console.log(this.groups);
    //     })
    //   }
    // })
  }
}


