import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { GroupModel } from '../../models/group';

@Component({
  selector: 'app-group-actions',
  templateUrl: './group-actions.component.html',
  styleUrl: './group-actions.component.css'
})
export class GroupActionsComponent {

  constructor(
    public dialogRef: MatDialogRef<GroupActionsComponent>,
    @Inject(MAT_DIALOG_DATA) public data:
     {
      group:GroupModel,
      deleteGroups: (group:GroupModel) => void,
    },
    private dialog: MatDialog
  ) {

  }


  deleteGroup(){
    this.data.deleteGroups(this.data.group)
    this.dialogRef.close()

  }


}
