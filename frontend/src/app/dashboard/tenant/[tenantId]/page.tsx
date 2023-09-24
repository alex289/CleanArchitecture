'use client';

import { redirect } from 'next/navigation';

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

export default function TenantPage({
  params,
}: {
  params: { tenantId: string };
}) {
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
      redirect('/dashboard');
    }
  }

  return (
    <main>
      <div className="flex items-center justify-between space-y-2">
        <h2 className="text-2xl font-bold tracking-tight">
          {tenant.data.data?.name}
        </h2>
        <AlertDialog>
          <AlertDialogTrigger>
            <Button variant="destructive">Delete</Button>
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
      <div className="container mx-auto py-10">
        <UserTable data={tenant.data.data?.users ?? []} />
      </div>
    </main>
  );
}
