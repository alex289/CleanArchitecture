import { Settings } from 'lucide-react';

import { MainNav } from '@/components/main-nav';
import { ModeToggle } from '@/components/theme-toggle';

import type { ReactNode } from 'react';

export default function DashboardLayout({ children }: { children: ReactNode }) {
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
      <div className="mt-4 px-4">{children}</div>
    </div>
  );
}
