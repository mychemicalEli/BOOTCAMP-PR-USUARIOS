export interface PaginatedResponse<UserDto> {
    currentPage: number;
    totalPages: number;
    pageSize: number;
    totalCount: number;
    data: UserDto[];
  }
  
  export interface UserDto {
    id: number;
    name: string;
    lastName: string;
    email: string;
    roleId: number;
    roleName: string;
  }
  