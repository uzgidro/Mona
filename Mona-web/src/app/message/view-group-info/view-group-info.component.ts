import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { GroupModel } from '../../models/group';
import { UserModel } from '../../models/user';

@Component({
  selector: 'app-view-group-info',
  templateUrl: './view-group-info.component.html',
  styleUrl: './view-group-info.component.css'
})
export class ViewGroupInfoComponent {

  constructor(
    public dialogRef: MatDialogRef<ViewGroupInfoComponent>,
    @Inject(MAT_DIALOG_DATA) public data:
    {
     group:GroupModel,
     deleteGroups: (group:GroupModel) => void,
     leaveGroups: (group:GroupModel) => void,
     selectChat: (chat:UserModel) => void,
   },
   private dialog: MatDialog
  ) {
  }


  cancel(){
    this.dialogRef.close()
  }
  selectChat(user:UserModel){
    this.data.selectChat(user)
    this.cancel()
  }

}
