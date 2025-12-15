export interface User {
  id: string;
  username: string;
  email: string;
  role: string;
  employee?: {
    id: string;
    firstName: string;
    lastName: string;
    position: string;
  };
}

export interface BranchOffice {
  id: string;
  name: string;
  address: string;
  city: string;
  country: string;
  phoneNumber: string;
  email: string;
  createdAt: string;
  updatedAt?: string;
}

export interface Employee {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  position: string;
  branchOfficeId?: string;
  departmentId?: string;
  hireDate: string;
  createdAt: string;
  updatedAt?: string;
  branchOffice?: BranchOffice;
  department?: Department;
}

export interface Department {
  id: string;
  name: string;
  description: string;
  branchOfficeId: string;
  managerId?: string;
  createdAt: string;
  updatedAt?: string;
  branchOffice?: BranchOffice;
}

export enum TaskStatus {
  Pending = 0,
  InProgress = 1,
  Completed = 2,
  Cancelled = 3
}

export enum TaskPriority {
  Low = 0,
  Medium = 1,
  High = 2,
  Critical = 3
}

export interface OfficeTask {
  id: string;
  title: string;
  description: string;
  status: TaskStatus;
  priority: TaskPriority;
  branchOfficeId?: string;
  departmentId?: string;
  assignedEmployeeId?: string;
  dueDate?: string;
  createdAt: string;
  updatedAt?: string;
  completedAt?: string;
  branchOffice?: BranchOffice;
  department?: Department;
  assignedEmployee?: Employee;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}


