import { MatDialogRef,MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { MessageService } from '../../services/message.service';
import { MessageModel } from '../../models/message';

@Component({
  selector: 'app-delete-message',
  templateUrl: './delete-message.component.html',
  styleUrl: './delete-message.component.css'
})
export class DeleteMessageComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteMessageComponent>,
    @Inject(MAT_DIALOG_DATA) public data:{message: MessageModel},
    private messageService:MessageService
  ) {}




  deleteMessageForEveryOne(){
    this.messageService.deleteMessageForEveryone(this.data.message.id)
    this.dialogRef.close()
  }


  deleteMessageForMyself(){
    this.messageService.deleteMessageForMyself(this.data.message.id)
    this.dialogRef.close()
  }




}
