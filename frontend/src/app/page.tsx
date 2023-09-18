'use client';

import { useAuth } from '@/lib/use-auth';
import { redirect } from 'next/navigation';

export default function Home() {
  const { loading, loggedOut } = useAuth();

  if (loading) {
    return <main className="p-4">Loading...</main>;
  }

  if (loggedOut) {
    redirect('/login');
  }

  redirect('/dashboard');
  return <main className="p-4">Redirecting</main>;
}
