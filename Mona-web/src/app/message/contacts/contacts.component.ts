import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MessageModel } from '../../models/message';
import { UserModel } from '../../models/user';
import { ApiService } from '../../services/api.service';
import { AddContactComponent } from './add-contact/add-contact.component';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrl: './contacts.component.css'
})
export class ContactsComponent {

  constructor(
    public dialogRef: MatDialogRef<ContactsComponent>,
    @Inject(MAT_DIALOG_DATA) public data:
     {
      users:UserModel[],
      selectChat: (user: UserModel) => void,
    },
    private apiService:ApiService,
    private dialog: MatDialog,
  ) {
  }



  cancel(){
    this.dialogRef.close()
  }




  onSelectUser(user:UserModel){
    this.data.selectChat(user)
    this.cancel()
  }

  addContact(){
    this.cancel()
   const dialogRef= this.dialog.open(AddContactComponent, {
      width: '400px',
      data:
       {
        },


    });
    dialogRef.afterClosed().subscribe(() => {
    })

  }



}
