import api from './axiosConfig';
import type { BranchOffice } from '../types';

export interface BranchOfficeFilter {
  searchTerm?: string;
  headOfficeId?: string;
  city?: string;
  page?: number;
  pageSize?: number;
}

export const branchOfficeService = {
  getAll: async (filters?: BranchOfficeFilter): Promise<BranchOffice[]> => {
    const response = await api.get<BranchOffice[]>('/BranchOffice', { params: filters });
    return response.data;
  },

  getById: async (id: string): Promise<BranchOffice> => {
    const response = await api.get<BranchOffice>(`/BranchOffice/${id}`);
    return response.data;
  },

  create: async (data: Omit<BranchOffice, 'id' | 'createdAt' | 'updatedAt' | 'headOffice'>): Promise<BranchOffice> => {
    const response = await api.post<BranchOffice>('/BranchOffice', data);
    return response.data;
  },

  update: async (id: string, data: Omit<BranchOffice, 'id' | 'createdAt' | 'updatedAt' | 'headOffice'>): Promise<BranchOffice> => {
    const response = await api.put<BranchOffice>(`/BranchOffice/${id}`, data);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/BranchOffice/${id}`);
  },
};



