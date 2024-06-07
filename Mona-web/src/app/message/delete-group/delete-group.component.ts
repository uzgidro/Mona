import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { GroupModel } from '../../models/group';

@Component({
  selector: 'app-delete-group',
  templateUrl: './delete-group.component.html',
  styleUrl: './delete-group.component.css'
})
export class DeleteGroupComponent {

  isDeleteForEveryone:boolean=false

  constructor(
    public dialogRef: MatDialogRef<DeleteGroupComponent>,
    @Inject(MAT_DIALOG_DATA) public data:
     {
      group:GroupModel,
      deleteGroups: (group:GroupModel) => void,
      leaveGroups: (group:GroupModel) => void,
    },
    private dialog: MatDialog
  ) {

  }


  cancel(){
    this.dialogRef.close()

  }

  deleteGroup(){
    if (this.isDeleteForEveryone) {
    this.data.deleteGroups(this.data.group)
    } else {
    this.data.leaveGroups(this.data.group)
    }

    this.cancel()
  }

}
