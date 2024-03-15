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
    username: string | null;
    password: string | null;
    passwordRepeat: string | null;
  }>) {
    if (userForm.firstName && userForm.lastName && userForm.password && userForm.username) {
      const model: User = {
        firstName: userForm.firstName,
        lastName: userForm.lastName,
        username: userForm.username,
        password: userForm.password
      }
      return model
    }
    return undefined
  }
}
