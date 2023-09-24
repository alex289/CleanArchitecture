import Link from 'next/link';

import { cn } from '@/lib/utils';

import type { HTMLAttributes } from 'react';

export function MainNav({ className, ...props }: HTMLAttributes<HTMLElement>) {
  return (
    <nav
      className={cn('flex items-center space-x-4 lg:space-x-6', className)}
      {...props}>
      <Link
        href="/dashboard"
        className="text-sm font-medium transition-colors hover:text-primary">
        Tenants
      </Link>
      <Link
        href="/dashboard/user"
        className="text-sm font-medium transition-colors hover:text-primary">
        Users
      </Link>
    </nav>
  );
}
