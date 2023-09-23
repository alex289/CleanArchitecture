'use client';

import { redirect } from 'next/navigation';
import { Settings } from 'lucide-react';

import { useAuth } from '@/lib/use-auth';
import { MainNav } from '@/components/main-nav';
import { ModeToggle } from '@/components/theme-toggle';

import type { ReactNode } from 'react';

export default function DashboardLayout({ children }: { children: ReactNode }) {
  const { loading, loggedOut } = useAuth();

  if (loggedOut) {
    redirect('/login');
  }

  return (
    <div>
      <div className="border-b">
        <div className="flex h-16 items-center px-4">
          <MainNav className="mx-6" />
          <div className="ml-auto mr-4 flex items-center space-x-4">
            <ModeToggle />
            <button>
              <Settings />
            </button>
          </div>
        </div>
      </div>
      <div className="mt-4 px-4">
        {loading ? <div className="m-4">Loading...</div> : children}
      </div>
    </div>
  );
}
