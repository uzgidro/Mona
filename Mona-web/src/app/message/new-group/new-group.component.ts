import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { UserModel } from '../../models/user';
import { ApiService } from '../../services/api.service';
import { AddMembersComponent } from './add-members/add-members.component';
import { GroupModel, GroupRequest } from '../../models/group';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-new-group',
  templateUrl: './new-group.component.html',
  styleUrl: './new-group.component.css'
})
export class NewGroupComponent {

  myGroup: FormGroup;



  constructor(
    public dialogRef: MatDialogRef<NewGroupComponent>,
    @Inject(MAT_DIALOG_DATA) public data:
     {
      users:UserModel[],
      createGroup: (groupRequest: GroupRequest) => void,
    },
    private apiService:ApiService,
    private dialog: MatDialog,
  ) {

    this.myGroup = new FormGroup({
      groupName: new FormControl(''),
      file: new FormControl('')
    });
  }






  cancel(){
    this.dialogRef.close()
  }


  createGroups(groupRequest: GroupRequest){
    this.data.createGroup(groupRequest)
  }

  addMembers(){
    let groupName = this.myGroup.get('groupName')?.value;
    this.cancel()
    const dialogRef= this.dialog.open(AddMembersComponent, {
       width: '400px',
       data:
        {
          users:this.data.users,
          createGroup:this.createGroups.bind(this),
          groupName:groupName
         },


     });
     dialogRef.afterClosed().subscribe(() => {
     })


  }



}
