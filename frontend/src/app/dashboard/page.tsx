'use client';

import { redirect } from 'next/navigation';
import dynamic from 'next/dynamic';
import useSWR from 'swr';

import { useAuth } from '@/lib/use-auth';
import { fetcher } from '@/lib/fetcher';

import type { TenantModel } from '@/types/tenant.model';
import type { ApiResponse } from '@/types/api-response';
import type { PagedResult } from '@/types/paged-result';

const TenantTable = dynamic(() => import('@/components/tables/tenant-table'), {
  ssr: false,
});

export default function Home() {
  const { loading, loggedOut, user } = useAuth();
  const tenants = useSWR<ApiResponse<PagedResult<TenantModel>>>(
    `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/v1/tenant`,
    fetcher,
  );

  if (loading || tenants.isLoading) {
    return <div>Loading...</div>;
  }

  if (loggedOut) {
    redirect('/login');
  }

  if (tenants.error || !tenants.data?.success) {
    return <div>Error loading tenants :(</div>;
  }

  return (
    <main>
      <div className="flex items-center justify-between space-y-2">
        <h2 className="text-2xl font-bold tracking-tight">
          Welcome back, {user?.firstName}!
        </h2>
      </div>
      {tenants?.data?.data && (
        <div className="container mx-auto py-10">
          <TenantTable data={tenants?.data.data.items} />
        </div>
      )}
    </main>
  );
}
