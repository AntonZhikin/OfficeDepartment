import api from './axiosConfig';
import type { OfficeTask, TaskStatus, TaskPriority } from '../types';

export interface TaskFilter {
  searchTerm?: string;
  status?: TaskStatus;
  priority?: TaskPriority;
  branchOfficeId?: string;
  assignedEmployeeId?: string;
  page?: number;
  pageSize?: number;
}

export const taskService = {
  getAll: async (filters?: TaskFilter): Promise<OfficeTask[]> => {
    const response = await api.get<OfficeTask[]>('/OfficeTask', { params: filters });
    return response.data;
  },

  getById: async (id: string): Promise<OfficeTask> => {
    const response = await api.get<OfficeTask>(`/OfficeTask/${id}`);
    return response.data;
  },

  create: async (data: Omit<OfficeTask, 'id' | 'createdAt' | 'updatedAt' | 'completedAt' | 'branchOffice' | 'assignedEmployee'>): Promise<OfficeTask> => {
    const response = await api.post<OfficeTask>('/OfficeTask', data);
    return response.data;
  },

  update: async (id: string, data: Partial<Omit<OfficeTask, 'id' | 'createdAt' | 'updatedAt' | 'branchOffice' | 'assignedEmployee'>>): Promise<OfficeTask> => {
    const response = await api.put<OfficeTask>(`/OfficeTask/${id}`, data);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/OfficeTask/${id}`);
  },
};

