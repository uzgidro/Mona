import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { UserModel } from '../../models/user';
import { ApiService } from '../../services/api.service';
import { AddContactComponent } from '../contacts/add-contact/add-contact.component';
import { AddMembersComponent } from './add-members/add-members.component';
import { GroupModel, GroupRequest } from '../../models/group';

@Component({
  selector: 'app-new-group',
  templateUrl: './new-group.component.html',
  styleUrl: './new-group.component.css'
})
export class NewGroupComponent {

groupName:string=''

  constructor(
    public dialogRef: MatDialogRef<NewGroupComponent>,
    @Inject(MAT_DIALOG_DATA) public data:
     {

      users:UserModel[],
      groups:GroupModel[],
      createGroup: (groupRequest: GroupRequest) => void,
    },
    private apiService:ApiService,
    private dialog: MatDialog,
  ) {
  }



  cancel(){
    this.dialogRef.close()
  }


  createGroups(groupRequest: GroupRequest){
    this.data.createGroup(groupRequest)
  }

  addMembers(){

    this.cancel()
    const dialogRef= this.dialog.open(AddMembersComponent, {
       width: '400px',
       data:
        {
     
          users:this.data.users,
          createGroup:this.createGroups.bind(this),
          groupName:this.groupName
         },


     });
     dialogRef.afterClosed().subscribe(() => {
     })


  }



}
