export interface PaginatedResponse<UserDto> {
    currentPage: number;
    totalPages: number;
    pageSize: number;
    totalCount: number;
    data: UserDto[];
  }
  
  export interface UserDto {
    id: number | undefined;
    name: string;
    lastName: string;
    email: string;
    roleId: number;
    roleName: string;
    rowVersion: string | null;
  }
  