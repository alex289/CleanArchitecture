'use client';

import { useAuth } from '@/lib/use-auth';
import { useRouter } from 'next/navigation';

export default function Home() {
  const router = useRouter();
  const { loading, loggedOut, user } = useAuth();

  if (loading) {
    return <div>Loading...</div>;
  }

  if (loggedOut) {
    router.push('/login');
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
