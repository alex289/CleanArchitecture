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

  return <main>{JSON.stringify(user)}</main>;
}
