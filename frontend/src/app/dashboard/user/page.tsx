'use client';

import UserTable from '@/components/tables/user-table';
import { useAPI } from '@/lib/use-api';

import type { PagedResult } from '@/types/paged-result';
import type { UserModel } from '@/types/user.model';

export default function UsersPage() {
  const users = useAPI<PagedResult<UserModel>>('user');

  if (users.isLoading) {
    return <div className="m-4">Loading users...</div>;
  }

  if (users.error || !users.data?.success) {
    return <div className="m-4">Error loading users :(</div>;
  }
  return (
    <main>
      <div className="flex items-center justify-between space-y-2">
        <h2 className="text-2xl font-bold tracking-tight">Users</h2>
      </div>
      {users?.data?.data && (
        <div className="container mx-auto py-10">
          <UserTable data={users?.data.data.items} />
        </div>
      )}
    </main>
  );
}
