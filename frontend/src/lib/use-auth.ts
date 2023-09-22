'use client';

import useSWR from 'swr';

import { fetcher } from './fetcher';

import type { ApiResponse } from '@/types/api-response';
import type { UserModel } from '@/types/user.model';

export function useAuth() {
  const { data, error, mutate } = useSWR<ApiResponse<UserModel>>(
    `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/v1/user/me`,
    fetcher,
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
