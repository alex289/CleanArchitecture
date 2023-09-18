'use client';

import { redirect } from 'next/navigation';

import { useAuth } from '@/lib/use-auth';

export default function Home() {
  const { loading, loggedOut, user } = useAuth();

  if (loading) {
    return <div>Loading...</div>;
  }

  if (loggedOut) {
    redirect('/login');
  }

  return (
    <main>
      <div className="flex items-center justify-between space-y-2">
        <div>
          <h2 className="text-2xl font-bold tracking-tight">
            Welcome back, {user?.firstName}!
          </h2>
        </div>
      </div>
    </main>
  );
}
