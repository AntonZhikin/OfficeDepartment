import api from './axiosConfig';
import type { LoginRequest, RegisterRequest, AuthResponse } from '../types';

export const authService = {
  login: async (data: LoginRequest): Promise<AuthResponse> => {
    const response = await api.post<AuthResponse>('/auth/login', data);
    return response.data;
  },

  register: async (data: RegisterRequest): Promise<{ message: string; userId: string }> => {
    const response = await api.post('/auth/register', data);
    return response.data;
  },
};

