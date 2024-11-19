import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginatedResponse, UserDto } from '../model/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  public getUsers(page: number, size: number, filters?: string): Observable<PaginatedResponse<UserDto>> {
    let urlEndPoint = `http://localhost:5085/users?PageNumber=${page}&PageSize=${size}`;
    if (filters) {
      urlEndPoint = urlEndPoint + "&filter=" + filters;
    }
    return this.http.get<PaginatedResponse<UserDto>>(urlEndPoint);
  }

  public deleteUser(userIdToDelete: number): Observable<any> {
    let urlEndPoint: string = "http://localhost:5085/users/" + userIdToDelete;
    return this.http.delete<any>(urlEndPoint);
  }


}
