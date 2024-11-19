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

  nameFilter?: string;
  lastNameFilter?: string;
  roleFilter?: string;

  noResultsFound: boolean = false;

  selectedUser: { name: string; lastName: string } | null = null;
  userIdToDelete?: number;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.getUsers();
  }

  private getUsers(): void {
    const filters: string | undefined = this.buildFilters();
    this.userService.getUsers(this.currentPage, this.pageSize, filters).subscribe({
      next: (response: PaginatedResponse<UserDto>) => {
        this.users = response.data;
        this.currentPage = response.currentPage;
        this.totalPages = response.totalPages;
        this.pageSize = response.pageSize;
        this.totalCount = response.totalCount;
        this.noResultsFound = this.users.length === 0;
      },
      error: (err) => { this.handleError(err); }
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

  private buildFilters(): string | undefined {
    const filters: string[] = [];

    if (this.nameFilter) filters.push(`name:MATCH:${this.nameFilter}`);
    if (this.lastNameFilter) filters.push(`lastName:MATCH:${this.lastNameFilter}`);
    if (this.roleFilter) filters.push(`role.Name:MATCH:${this.roleFilter}`);

    return filters.length > 0 ? filters.join(",") : undefined;
  }


  public searchByFilters(): void {
    this.currentPage = 1;
    this.getUsers();
  }

  public resetFilters(): void {
    this.nameFilter = '';
    this.lastNameFilter = '';
    this.roleFilter = '';
    this.searchByFilters();
  }

  setSelectedUser(user: { name: string; lastName: string }): void {
    this.selectedUser = user;
  }

  public prepareUserToDelete(userId:number):void{
    this.userIdToDelete=userId;
  }

  public deleteUser():void{
    if(this.userIdToDelete){
    this.userService.deleteUser(this.userIdToDelete).subscribe({
      next: (data)=>{
        this.getUsers();
      },
      error:(err)=> {this.handleError(err)}
    });
  }
}

}
