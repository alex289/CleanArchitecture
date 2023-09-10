'use client';

import { useRouter } from 'next/navigation';

export default function Home() {
  const router = useRouter();

  if (!localStorage.getItem('auth_token')) {
    router.push('/login');
  }
  return <main></main>;
}
