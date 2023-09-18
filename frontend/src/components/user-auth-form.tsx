'use client';

import { type HTMLAttributes, useState } from 'react';
import { useRouter } from 'next/navigation';

import { cn } from '@/lib/utils';
import { Button } from './ui/button';
import { Label } from './ui/label';
import { Input } from './ui/input';
import { Icons } from './icons';

import type { ApiResponse } from '@/types/api-response';

interface UserAuthFormProps extends HTMLAttributes<HTMLDivElement> {}

export function UserAuthForm({ className, ...props }: UserAuthFormProps) {
  const router = useRouter();
  const [isLoading, setIsLoading] = useState<boolean>(false);

  async function onSubmit(event: React.SyntheticEvent) {
    event.preventDefault();
    setIsLoading(true);

    const res = await fetch(
      `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/v1/user/login`,
      {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          email: (event.target as any).email.value,
          password: (event.target as any).password.value,
        }),
      },
    );

    if (res.ok) {
      const body = (await res.json()) as ApiResponse<string>;
      localStorage.setItem('auth_token', body.data!);
      router.push('/dashboard');
    }

    setTimeout(() => {
      setIsLoading(false);
    }, 3000);
  }

  return (
    <div className={cn('grid gap-6', className)} {...props}>
      <form onSubmit={onSubmit}>
        <div className="grid gap-2">
          <div className="grid gap-1">
            <Label className="sr-only" htmlFor="email">
              Email
            </Label>
            <Input
              id="email"
              placeholder="Email"
              type="email"
              autoCapitalize="none"
              autoComplete="email"
              autoCorrect="off"
              disabled={isLoading}
            />
          </div>
          <div className="grid gap-1">
            <Label className="sr-only" htmlFor="password">
              Password
            </Label>
            <Input
              id="password"
              placeholder="Password"
              type="password"
              autoCapitalize="none"
              autoComplete="password"
              autoCorrect="off"
              disabled={isLoading}
            />
          </div>
          <Button disabled={isLoading}>
            {isLoading && (
              <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />
            )}
            Sign In
          </Button>
        </div>
      </form>
    </div>
  );
}
