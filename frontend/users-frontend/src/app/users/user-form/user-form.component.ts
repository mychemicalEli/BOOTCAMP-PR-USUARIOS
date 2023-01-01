import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../service/user.service';
import { UserDto } from '../model/user.model';
import { RoleDto } from '../model/role.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

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

  userForm?: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private router: Router,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.initializeUser();
    this.buildForm();
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
      next: (userRequest) => {
        this.user = userRequest;
        this.updateForm(userRequest);
      },
      error: (err) => { this.handleError(err); }
    });
  }

  public getRoles(): void {
    this.userService.getRoles().subscribe({
      next: (roles) => {
        this.roles = roles;
      },
      error: (err) => { this.handleError(err); }
    });
  }

  public handleError(error: any): void {
    console.log(error);
  }

  public saveUser(): void {
    const itemToSave: UserDto=this.createFromForm();
    if (this.mode === "CREAR NUEVO USUARIO") {
      this.insertUser(itemToSave);
    }

    if (this.mode === "EDITAR USUARIO") {
      this.updateUser(itemToSave);
    }
  }

  private insertUser(userToSave: UserDto): void {
    this.userService.insertUser(userToSave).subscribe({
      next: (userInserted) => {
        console.log("Insertado correctamente", userInserted);
        this.router.navigate(['/']);
      },
      error: (err) => { this.handleError(err); }
    });
  }
  
  private updateUser(userToSave: UserDto): void {
    this.userService.updateUser(userToSave).subscribe({
      next: (userUpdated) => {
        console.log("Actualizado correctamente", userUpdated);
        this.router.navigate(['/']);
      },
      error: (err) => { this.handleError(err); }
    });
  }

  private initializeUser() {
    this.user = {
      id: undefined,
      name: '',
      lastName: '',
      email: '',
      roleId: 0,
      roleName: '',
      rowVersion: ''
    };
    this.userForm?.patchValue(this.user);
  }

  public goBack(): void {
    this.router.navigate(['/']);
  }

  public buildForm(): void {
    this.userForm = this.fb.group({
      id: [{ value: undefined, disabled: true }],
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      email: ['', [
        Validators.required,
        Validators.email,
        Validators.minLength(5),
        Validators.maxLength(100)
      ]],
      roleId: [''],
      roleName: [''],
      rowVersion: [{ value: undefined, disabled: true }]
    })
  }
  

  private updateForm(user: UserDto): void {
    this.userForm?.patchValue({
      id: user.id,
      name: user.name,
      lastName: user.lastName,
      email: user.email,
      roleId: user.roleId,
      roleName: user.roleName,
      rowVersion: user.rowVersion
    });
  }

  private createFromForm(): UserDto {
    return {
      ...this.user,
      id: this.userForm?.get('id')!.value,
      name: this.userForm?.get('name')!.value,
      lastName: this.userForm?.get('lastName')!.value,
      email: this.userForm?.get('email')!.value,
      roleId: this.userForm?.get('roleId')!.value,
      roleName: this.userForm?.get('roleName')!.value,
      rowVersion: this.userForm?.get('rowVersion')!.value,
    };
  }
}
