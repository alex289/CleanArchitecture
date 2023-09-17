'use client';

import { ApiResponse } from '@/types/api-response';
import { UserModel } from '@/types/user.model';
import useSWR from 'swr';

const authFetcher = (url: string) =>
  fetch(url, {
    headers: {
      Authorization: `Bearer ${localStorage.getItem('auth_token')}`,
    },
  }).then((r) => r.json());

export function useAuth() {
  const { data, error, mutate } = useSWR<ApiResponse<UserModel>>(
    `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/v1/user/me`,
    authFetcher,
  );

  const loading = !data && !error;
  const loggedOut = error && error.status === 403;

  return {
    loading,
    loggedOut,
    user: data?.data,
    mutate,
  };
}
