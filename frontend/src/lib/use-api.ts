import useSWR from 'swr';

import { fetcher } from './fetcher';

import type { ApiResponse } from '@/types/api-response';

export const useAPI = <T>(path: string) =>
  useSWR<ApiResponse<T>>(
    `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/v1/${path}`,
    fetcher,
  );
