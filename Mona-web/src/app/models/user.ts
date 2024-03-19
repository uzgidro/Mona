export interface User {
  firstName: string
  lastName: string
  username: string
  password: string
}

export interface UserModel{
  firstName: string;
  lastName: string;
  id: string;
  userName: string;
  normalizedUserName: string;
  email?: any;
  normalizedEmail?: any;
  emailConfirmed: boolean;
  passwordHash: string;
  securityStamp: string;
  concurrencyStamp: string;
  phoneNumber?: any;
  phoneNumberConfirmed: boolean;
  twoFactorEnabled: boolean;
  lockoutEnd?: any;
  lockoutEnabled: boolean;
  accessFailedCount: number;
}
