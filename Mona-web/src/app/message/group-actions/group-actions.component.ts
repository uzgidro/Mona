import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';

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

    },
    private dialog: MatDialog
  ) {

  }


}
