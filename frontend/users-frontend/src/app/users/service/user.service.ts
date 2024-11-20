import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginatedResponse, UserDto } from '../model/user.model';
import { RoleDto } from '../model/role.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly BASE_URL = 'http://localhost:5085/users';

  constructor(private http: HttpClient) { }

  public getUsers(page: number, size: number, filters?: string): Observable<PaginatedResponse<UserDto>> {
    let urlEndPoint = `${this.BASE_URL}?PageNumber=${page}&PageSize=${size}`;
    if (filters) {
      urlEndPoint += `&filter=${filters}`;
    }
    return this.http.get<PaginatedResponse<UserDto>>(urlEndPoint);
  }

  public deleteUser(userIdToDelete: number): Observable<any> {
    const urlEndPoint = `${this.BASE_URL}/${userIdToDelete}`;
    return this.http.delete<any>(urlEndPoint);
  }

  public getUserById(userId: number): Observable<UserDto> {
    const urlEndPoint = `${this.BASE_URL}/${userId}`;
    return this.http.get<UserDto>(urlEndPoint);
  }

  public insertUser(user: UserDto): Observable<UserDto> {
    const urlEndPoint = `${this.BASE_URL}/`;
    return this.http.post<UserDto>(urlEndPoint, user);
  }

  public updateUser(user: UserDto): Observable<UserDto> {
    const urlEndPoint = `${this.BASE_URL}/`;
    return this.http.put<UserDto>(urlEndPoint, user);
  }

  public getRoles(): Observable<RoleDto[]> {
    const urlEndPoint = `${this.BASE_URL}/roles`;
    return this.http.get<RoleDto[]>(urlEndPoint);
  }
}
