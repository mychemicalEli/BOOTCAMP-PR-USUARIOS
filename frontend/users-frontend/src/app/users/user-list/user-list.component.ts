import { Component, OnInit } from '@angular/core';
import { PaginatedResponse, UserDto } from '../model/user.model';
import { UserService } from '../service/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent implements OnInit {

  users: UserDto[] = [];
  currentPage: number = 1;
  totalPages: number = 0;
  pageSize: number = 10;
  totalCount: number = 0;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.getUsers();
  }

  private getUsers(): void {
    this.userService.getUsers(this.currentPage, this.pageSize).subscribe({
      next: (response: PaginatedResponse<UserDto>) => {
        this.users = response.data;
        this.currentPage = response.currentPage;
        this.totalPages = response.totalPages;
        this.pageSize = response.pageSize;
        this.totalCount = response.totalCount;
      },
      error: (err) => {
        console.error(this.handleError(err));
      }
    });
  }

  private handleError(error: any): void {
    console.log(error);
  }

  public previousPage(): void {
    this.currentPage = this.currentPage - 1;
    this.getUsers();
  }
  public nextPage(): void {
    this.currentPage = this.currentPage + 1;
    this.getUsers();
  }

}
