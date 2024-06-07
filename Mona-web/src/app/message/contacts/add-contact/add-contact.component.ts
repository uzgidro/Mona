import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserModel } from '../../../models/user';
import { ApiService } from '../../../services/api.service';

@Component({
  selector: 'app-add-contact',
  templateUrl: './add-contact.component.html',
  styleUrl: './add-contact.component.css'
})
export class AddContactComponent {

  constructor(
    public dialogRef: MatDialogRef<AddContactComponent>,
    @Inject(MAT_DIALOG_DATA) public data:
     {
      users:UserModel[],
      selectChat: (user: UserModel) => void,
    },
    private apiService:ApiService,
  ) {
  }


  cancel(){
    this.dialogRef.close()
  }





}
