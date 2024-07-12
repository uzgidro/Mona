import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MessageModel } from '../../models/message';
import { UserModel,GetUserResponse } from '../../models/user';
import { ApiService } from '../../services/api.service';
import { AddContactComponent } from './add-contact/add-contact.component';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrl: './contacts.component.css'
})
export class ContactsComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ContactsComponent>,
    @Inject(MAT_DIALOG_DATA) public data:
     {
      users:GetUserResponse[],
      selectContact: (user:GetUserResponse) => void,
    },
    private apiService:ApiService,
    private dialog: MatDialog,
  ) {
  }


  ngOnInit(): void {
    console.log(this.data.users);

  }


  cancel(){
    this.dialogRef.close()
  }




  onSelectUser(user:GetUserResponse){
    this.data.selectContact(user)
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
