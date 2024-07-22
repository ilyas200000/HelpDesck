import { Injectable } from '@angular/core';
import {User} from '../_models/user/user.module'
export interface AuthResponseData {
  access_token: string;
  expires_in: number;
  token_type: string;
  scope: string;
}

const TOKEN_KEY = 'access-token';
const USER_KEY = 'authenticated-user';

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  constructor() { }

  clean(): void {
    window.sessionStorage.clear();
    window.localStorage.clear();
  }

  saveUser(user: User): void {
    try {
      window.localStorage.removeItem(USER_KEY);
      window.localStorage.setItem(USER_KEY, JSON.stringify(user));
    } catch (error) {
      console.error('Error saving user to localStorage', error);
    }
  }

  getUser(): User | null {
    try {
      const user = window.localStorage.getItem(USER_KEY);
      if (user) {
        return JSON.parse(user);
      }
      return null;
    } catch (error) {
      console.error('Error retrieving user from localStorage', error);
      return null;
    }
  }

  saveAuthResponseData(authResponseData: AuthResponseData): void {
    try {
      window.localStorage.removeItem(TOKEN_KEY);
      window.localStorage.setItem(TOKEN_KEY, JSON.stringify(authResponseData));
    } catch (error) {
      console.error('Error saving auth response data to localStorage', error);
    }
  }
  getUserRoles(): string[] {
    const user = this.getUser();
    return user ? user.role : [];
  }
  getUserEntityId(): number {
    const user = this.getUser();
  
    return user ? user.entite_id : 0;
  }




}
