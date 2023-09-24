'use client';

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

  return (
    <main>
      <div className="flex items-center justify-between space-y-2">
        <h2 className="text-2xl font-bold tracking-tight">
          {tenant.data.data?.firstName} {tenant.data.data?.lastName}
        </h2>
      </div>
      <div className="container mx-auto py-10">
        <p>Email: {tenant.data.data?.email}</p>
        <p>Role: {role}</p>
        <p>Status: {status}</p>
      </div>
    </main>
  );
}
