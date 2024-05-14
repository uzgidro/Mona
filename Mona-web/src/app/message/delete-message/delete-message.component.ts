import { MatDialogRef,MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';

@Component({
  selector: 'app-delete-message',
  templateUrl: './delete-message.component.html',
  styleUrl: './delete-message.component.css'
})
export class DeleteMessageComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteMessageComponent>,
    @Inject(MAT_DIALOG_DATA) public data:{},
  ) {}




  deleteMessageForEveryOne(){
    this.dialogRef.close()

  }


  deleteMessageForMyself(){
    this.dialogRef.close()
  }




}
