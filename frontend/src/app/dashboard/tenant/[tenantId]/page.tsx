'use client';

import { useAPI } from '@/lib/use-api';

import type { TenantModel } from '@/types/tenant.model';

export default function TenantPage({
  params,
}: {
  params: { tenantId: string };
}) {
  const tenant = useAPI<TenantModel>(`/tenant/${params.tenantId}`);

  if (tenant.isLoading) {
    return <div className="m-4">Loading tenant...</div>;
  }

  if (tenant.error || !tenant.data?.success) {
    return <div className="m-4">Error loading tenant :(</div>;
  }

  return (
    <main>
      <div className="flex items-center justify-between space-y-2">
        <h2 className="text-2xl font-bold tracking-tight">
          {tenant.data.data?.name}
        </h2>
      </div>
    </main>
  );
}
