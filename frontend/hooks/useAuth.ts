'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import apiClient from '@/lib/api';
import { User, LoginRequest, RegisterRequest, AuthResponse } from '@/lib/types';

export function useAuth() {
  const router = useRouter();
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const checkAuth = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        setLoading(false);
        return;
      }

      try {
        const response = await apiClient.get<User>('/auth/me');
        setUser(response.data);
      } catch {
        localStorage.removeItem('token');
        setUser(null);
      } finally {
        setLoading(false);
      }
    };

    const timer = setTimeout(checkAuth, 100);
    return () => clearTimeout(timer);
  }, []);

  const login = async (credentials: LoginRequest) => {
    try {
      const response = await apiClient.post<AuthResponse>(
        '/auth/login',
        credentials,
      );
      localStorage.setItem('token', response.data.token);
      setUser(response.data.user);
      router.push('/dashboard');
    } catch (error) {
      throw new Error('Login failed');
    }
  };

  const register = async (data: RegisterRequest) => {
    try {
      const response = await apiClient.post<AuthResponse>(
        '/auth/register',
        data,
      );
      localStorage.setItem('token', response.data.token);
      setUser(response.data.user);
      router.push('/dashboard');
    } catch (error) {
      throw new Error('Registration failed');
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    setUser(null);
    router.push('/login');
  };

  return { user, loading, login, register, logout };
}
