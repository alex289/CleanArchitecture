'use client';

import { useRouter } from 'next/navigation';

export default function Home() {
  const router = useRouter();

  if (typeof window === 'undefined') {
    return <main></main>;
  }

  if (!localStorage.getItem('auth_token')) {
    router.push('/login');
  }
  return <main></main>;
}
