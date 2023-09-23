'use client';

import { useAPI } from './use-api';

import type { UserModel } from '@/types/user.model';

export function useAuth() {
  const { data, error, mutate } = useAPI<UserModel>('user/me');

  const loading = !data && !error;
  const loggedOut = !!error;

  return {
    loading,
    loggedOut,
    user: data?.data,
    mutate,
  };
}
