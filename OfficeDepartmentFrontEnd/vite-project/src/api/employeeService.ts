import api from './axiosConfig';
import type { Employee } from '../types';

export interface EmployeeFilter {
  searchTerm?: string;
  branchOfficeId?: string;
  departmentId?: string;
  position?: string;
  page?: number;
  pageSize?: number;
}

export const employeeService = {
  getAll: async (filters?: EmployeeFilter): Promise<Employee[]> => {
    const response = await api.get<Employee[]>('/Employee', { params: filters });
    return response.data;
  },

  getById: async (id: string): Promise<Employee> => {
    const response = await api.get<Employee>(`/Employee/${id}`);
    return response.data;
  },

  create: async (data: Omit<Employee, 'id' | 'createdAt' | 'updatedAt' | 'branchOffice' | 'department'>): Promise<Employee> => {
    const response = await api.post<Employee>('/Employee', data);
    return response.data;
  },

  update: async (id: string, data: Omit<Employee, 'id' | 'createdAt' | 'updatedAt' | 'branchOffice' | 'department'>): Promise<Employee> => {
    const response = await api.put<Employee>(`/Employee/${id}`, data);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/Employee/${id}`);
  },
};

