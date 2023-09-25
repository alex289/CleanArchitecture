'use client';

import { useRouter } from 'next/navigation';
import dynamic from 'next/dynamic';

import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from '@/components/ui/alert-dialog';
import { Button } from '@/components/ui/button';

import UserTable from '@/components/tables/user-table';
import { useAPI } from '@/lib/use-api';

import type { TenantModel } from '@/types/tenant.model';

const UpsertTenantDialog = dynamic(
  () => import('@/components/dialogs/upsert-tenant-dialog'),
  {
    ssr: false,
  },
);
const UpsertUserDialog = dynamic(
  () => import('@/components/dialogs/upsert-user-dialog'),
  {
    ssr: false,
  },
);

export default function TenantPage({
  params,
}: {
  params: { tenantId: string };
}) {
  const router = useRouter();
  const tenant = useAPI<TenantModel>(`tenant/${params.tenantId}`);

  if (tenant.isLoading) {
    return <div className="m-4">Loading tenant...</div>;
  }

  if (tenant.error || !tenant.data?.success) {
    return <div className="m-4">Error loading tenant :(</div>;
  }

  async function deleteTenant(event: React.SyntheticEvent) {
    event.preventDefault();

    const res = await fetch(
      `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/v1/tenant/${params.tenantId}`,
      {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('auth_token')}`,
        },
      },
    );

    if (res.ok) {
      router.push('/dashboard');
    }
  }

  return (
    <main>
      <div className="flex items-center justify-between space-y-2">
        <h2 className="text-2xl font-bold tracking-tight">
          {tenant.data.data?.name}
        </h2>

        <div>
          <UpsertTenantDialog isUpdating={true} tenantData={tenant.data.data} />
          <AlertDialog>
            <AlertDialogTrigger asChild>
              <Button variant="destructive" className="ml-4">
                Delete
              </Button>
            </AlertDialogTrigger>
            <AlertDialogContent>
              <AlertDialogHeader>
                <AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
                <AlertDialogDescription>
                  This action cannot be undone. This will permanently delete the
                  tenant and remove your data from our servers.
                </AlertDialogDescription>
              </AlertDialogHeader>
              <AlertDialogFooter>
                <AlertDialogCancel>Cancel</AlertDialogCancel>
                <AlertDialogAction onClick={deleteTenant}>
                  Continue
                </AlertDialogAction>
              </AlertDialogFooter>
            </AlertDialogContent>
          </AlertDialog>
        </div>
      </div>
      <div>
        <UpsertUserDialog isUpdating={false} tenantId={params.tenantId} />
      </div>
      <div className="container mx-auto py-10">
        <UserTable data={tenant.data.data?.users ?? []} />
      </div>
    </main>
  );
}
