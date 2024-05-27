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
    private apiService:ApiService,
    private dialog: MatDialog,
  ) {
  }


  addUserToGroup(userId:string){
    this.selectedUsers.push(userId)
  }


  createGroups(){
    const groupRequest={
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
