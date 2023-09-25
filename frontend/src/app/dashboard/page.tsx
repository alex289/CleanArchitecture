'use client';

import dynamic from 'next/dynamic';

import { useAPI } from '@/lib/use-api';
import { useAuth } from '@/lib/use-auth';

import type { TenantModel } from '@/types/tenant.model';
import type { PagedResult } from '@/types/paged-result';

const TenantTable = dynamic(() => import('@/components/tables/tenant-table'), {
  ssr: false,
});
const UpsertTenantDialog = dynamic(
  () => import('@/components/dialogs/upsert-tenant-dialog'),
  {
    ssr: false,
  },
);

export default function Home() {
  const { user } = useAuth();
  const tenants = useAPI<PagedResult<TenantModel>>('tenant');

  if (tenants.isLoading) {
    return <div className="m-4">Loading tenants...</div>;
  }

  if (tenants.error || !tenants.data?.success) {
    return <div className="m-4">Error loading tenants :(</div>;
  }

  return (
    <main>
      <div className="flex items-center justify-between space-y-2">
        <h2 className="text-2xl font-bold tracking-tight">
          Welcome back, {user?.firstName}!
        </h2>
        <UpsertTenantDialog isUpdating={false} />
      </div>
      {tenants?.data?.data && (
        <div className="container mx-auto py-10">
          <TenantTable data={tenants?.data.data.items} />
        </div>
      )}
    </main>
  );
}
