import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginatedResponse, UserDto } from '../model/user.model';
import { RoleDto } from '../model/role.model';

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

  public getUserById(userId:number): Observable<UserDto>{
    let urlEndPoint: string = "http://localhost:5085/users/" + userId;
    return this.http.get<UserDto>(urlEndPoint);
  }

  public insertUser(user: UserDto):Observable<UserDto>{
    let urlEndPoint: string = "http://localhost:5085/users/";
    return this.http.post<UserDto>(urlEndPoint, user);
  }

  public updateUser(user: UserDto):Observable<UserDto>{
    let urlEndPoint: string = "http://localhost:5085/users/";
    return this.http.put<UserDto>(urlEndPoint, user);
  }


  public getRoles():Observable<RoleDto[]>{
    let urlEndPoint: string = "http://localhost:5085/users/roles";
    return this.http.get<RoleDto[]>(urlEndPoint);
  }


}
