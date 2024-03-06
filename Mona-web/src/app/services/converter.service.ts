import {Injectable} from '@angular/core';
import {User} from "../models/user";

@Injectable({
  providedIn: 'root'
})
export class ConverterService {

  constructor() {
  }

  convertUserFormToModel(userForm: Partial<{
    firstName: string | null;
    lastName: string | null;
    personalId: string | null;
    password: string | null;
    passwordRepeat: string | null;
  }>) {
    if (userForm.firstName && userForm.lastName && userForm.password && userForm.personalId) {
      const model: User = {
        firstName: userForm.firstName,
        lastName: userForm.lastName,
        personalId: userForm.personalId,
        password: userForm.password
      }
      return model
    }
    return undefined
  }
}
