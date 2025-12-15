import api from './axiosConfig';
import type { Department } from '../types';

export interface DepartmentFilter {
  searchTerm?: string;
  headOfficeId?: string;
  page?: number;
  pageSize?: number;
}

export const departmentService = {
  getAll: async (filters?: DepartmentFilter): Promise<Department[]> => {
    const response = await api.get<Department[]>('/Department', { params: filters });
    return response.data;
  },

  getById: async (id: string): Promise<Department> => {
    const response = await api.get<Department>(`/Department/${id}`);
    return response.data;
  },

  create: async (data: Omit<Department, 'id' | 'createdAt' | 'updatedAt' | 'headOffice'>): Promise<Department> => {
    const response = await api.post<Department>('/Department', data);
    return response.data;
  },

  update: async (id: string, data: Omit<Department, 'id' | 'createdAt' | 'updatedAt' | 'headOffice'>): Promise<Department> => {
    const response = await api.put<Department>(`/Department/${id}`, data);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/Department/${id}`);
  },
};



