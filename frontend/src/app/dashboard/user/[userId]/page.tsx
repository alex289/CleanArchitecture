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

import { useAPI } from '@/lib/use-api';

import { UserRole } from '@/types/user-role.enum';
import { UserStatus } from '@/types/user-status.enum';

import type { UserModel } from '@/types/user.model';

export default function UserPage({ params }: { params: { userId: string } }) {
  const tenant = useAPI<UserModel>(`user/${params.userId}`);

  if (tenant.isLoading) {
    return <div className="m-4">Loading tenant...</div>;
  }

  if (tenant.error || !tenant.data?.success) {
    return <div className="m-4">Error loading tenant :(</div>;
  }

  const role = UserRole[tenant.data.data?.role!];
  const status = UserStatus[tenant.data.data?.status!];

  async function deleteUser(event: React.SyntheticEvent) {
    event.preventDefault();

    const res = await fetch(
      `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/v1/user/${params.userId}`,
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
          {tenant.data.data?.firstName} {tenant.data.data?.lastName}
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
              <AlertDialogAction onClick={deleteUser}>
                Continue
              </AlertDialogAction>
            </AlertDialogFooter>
          </AlertDialogContent>
        </AlertDialog>
      </div>
      <div className="container mx-auto py-10">
        <p>Email: {tenant.data.data?.email}</p>
        <p>Role: {role}</p>
        <p>Status: {status}</p>
      </div>
    </main>
  );
}
