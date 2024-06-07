import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ApiService } from '../../../services/api.service';
import { UserModel } from '../../../models/user';
import { GroupModel, GroupRequest } from '../../../models/group';

@Component({
  selector: 'app-add-members',
  templateUrl: './add-members.component.html',
  styleUrl: './add-members.component.css'
})
export class AddMembersComponent {
  selectedUsers:string[]=[]


  constructor(
    public dialogRef: MatDialogRef<AddMembersComponent>,
    @Inject(MAT_DIALOG_DATA) public data:
     {
      users:UserModel[],
      createGroup: (groupRequest: GroupRequest) => void,
      groupName:string
    },

  ) {
  }


  addUserToGroup(userId: string) {
    if (this.selectedUsers.includes(userId)) {
      this.selectedUsers = this.selectedUsers.filter(user => user !== userId);
    } else {
      this.selectedUsers = [...this.selectedUsers, userId];
    }

    console.log(this.selectedUsers);
  }

  isUserSelected(userId: string): boolean {
    return this.selectedUsers.some(id => id === userId);
  }



  createGroups(){
    const groupRequest:GroupRequest={
      name:this.data.groupName,
      description:'....',
      members:this.selectedUsers
    }

    console.log(groupRequest);
    this.data.createGroup(groupRequest)
    this.cancel()
  }












  cancel(){
    this.dialogRef.close()
  }


}
