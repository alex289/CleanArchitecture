'use client';

import useSWR from 'swr';

import type { ApiResponse } from '@/types/api-response';
import type { UserModel } from '@/types/user.model';

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
  const loggedOut = !!error;

  return {
    loading,
    loggedOut,
    user: data?.data,
    mutate,
  };
}
