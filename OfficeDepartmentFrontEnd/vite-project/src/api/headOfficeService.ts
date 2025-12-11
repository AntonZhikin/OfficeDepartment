import api from './axiosConfig';
import type { HeadOffice } from '../types';

export interface HeadOfficeFilter {
  searchTerm?: string;
  city?: string;
  country?: string;
  page?: number;
  pageSize?: number;
}

export const headOfficeService = {
  getAll: async (filters?: HeadOfficeFilter): Promise<HeadOffice[]> => {
    const response = await api.get<HeadOffice[]>('/HeadOffice', { params: filters });
    return response.data;
  },

  getById: async (id: string): Promise<HeadOffice> => {
    const response = await api.get<HeadOffice>(`/HeadOffice/${id}`);
    return response.data;
  },

  create: async (data: Omit<HeadOffice, 'id' | 'createdAt' | 'updatedAt'>): Promise<HeadOffice> => {
    const response = await api.post<HeadOffice>('/HeadOffice', data);
    return response.data;
  },

  update: async (id: string, data: Omit<HeadOffice, 'id' | 'createdAt' | 'updatedAt'>): Promise<HeadOffice> => {
    const response = await api.put<HeadOffice>(`/HeadOffice/${id}`, data);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/HeadOffice/${id}`);
  },
};

