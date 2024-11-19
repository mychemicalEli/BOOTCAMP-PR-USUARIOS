import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../service/user.service';
import { UserDto } from '../model/user.model';
import { RoleDto } from '../model/role.model';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrl: './user-form.component.scss'
})
export class UserFormComponent implements OnInit {
  mode: "CREAR NUEVO USUARIO" | "EDITAR USUARIO" = "CREAR NUEVO USUARIO";
  userId?: number;
  user?: UserDto;
  roles: RoleDto[] = [];

  constructor(private route: ActivatedRoute, private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    const entryParam: string = this.route.snapshot.paramMap.get("id") ?? "new";
    if (entryParam !== "new") {
      this.userId = +this.route.snapshot.paramMap.get("id")!;
      this.mode = "EDITAR USUARIO";
      this.getUserById(this.userId);
    } else {
      this.mode = "CREAR NUEVO USUARIO";
      this.initializeUser();
    }
    this.getRoles();
  }

  private getUserById(userId: number) {
    this.userService.getUserById(userId).subscribe({
      next: (userRequest) => { this.user = userRequest },
      error: (err) => { this.handleError(err); }
    });
  }

  public getRoles(): void {
    this.userService.getRoles().subscribe({
      next: (roles) => {
        this.roles = roles;
      },
      error: (err) => {this.handleError(err);}
    });
  }
  
  public handleError(error: any): void {
    console.log(error);
  }

  public saveUser():void{
    if(this.mode==="CREAR NUEVO USUARIO"){
      this.insertUser();
    }

    if(this.mode==="EDITAR USUARIO"){
      this.updateUser();
    }
  }

  private insertUser():void{
    this.userService.insertUser(this.user!).subscribe({
      next: (userInserted)=>{
        console.log("Insertado correctamente");
        console.log(userInserted);
        this.router.navigate(['/']);
      },
      error:(err)=>{this.handleError(err);}
    });
  }
  private updateUser():void{
    this.userService.updateUser(this.user!).subscribe({
      next: (userUpdated)=>{
        console.log("Actualizado correctamente");
        console.log(userUpdated);
        this.router.navigate(['/']);
      },
      error:(err)=>{this.handleError(err);}
    });
  }

  private initializeUser(){
    this.user = {
      id: undefined,       
      name: '',   
      lastName: '', 
      email: '',   
      roleId: 0,
      roleName:'',
      rowVersion: ''
    };
  }

  public goBack():void{
    this.router.navigate(['/']);
  }
}
