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

import { useAPI } from '@/lib/use-api';

import { UserRole } from '@/types/user-role.enum';
import { UserStatus } from '@/types/user-status.enum';

import type { UserModel } from '@/types/user.model';

const UpsertUserDialog = dynamic(
  () => import('@/components/dialogs/upsert-user-dialog'),
  {
    ssr: false,
  },
);

export default function UserPage({ params }: { params: { userId: string } }) {
  const router = useRouter();
  const user = useAPI<UserModel>(`user/${params.userId}`);

  if (user.isLoading) {
    return <div className="m-4">Loading user...</div>;
  }

  if (user.error || !user.data?.success) {
    return <div className="m-4">Error loading user :(</div>;
  }

  const role = UserRole[user.data.data?.role!];
  const status = UserStatus[user.data.data?.status!];

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
      router.push('/dashboard');
    }
  }

  return (
    <main>
      <div className="flex items-center justify-between space-y-2">
        <h2 className="text-2xl font-bold tracking-tight">
          {user.data.data?.firstName} {user.data.data?.lastName}
        </h2>

        <div>
          <UpsertUserDialog isUpdating={true} userData={user.data.data} />
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
                <AlertDialogAction onClick={deleteUser}>
                  Continue
                </AlertDialogAction>
              </AlertDialogFooter>
            </AlertDialogContent>
          </AlertDialog>
        </div>
      </div>
      <div className="container mx-auto py-10">
        <p>Email: {user.data.data?.email}</p>
        <p>Role: {role}</p>
        <p>Status: {status}</p>
      </div>
    </main>
  );
}
