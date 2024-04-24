import { Injectable } from "@angular/core";
import { ApiService } from "./api.service";


@Injectable({
  providedIn: 'root'
})
export class MessageService{
  constructor(private apiService:ApiService){}






  sendMessage(formData:any){
    this.apiService.sendMessage(formData)
  }








}
