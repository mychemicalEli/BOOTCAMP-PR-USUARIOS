import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginatedResponse, UserDto } from '../model/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  public getUsers(page: number, size: number): Observable<PaginatedResponse<UserDto>> {
    const url = `http://localhost:5085/users?PageNumber=${page}&PageSize=${size}`;
    return this.http.get<PaginatedResponse<UserDto>>(url);
  }
  
  
}
