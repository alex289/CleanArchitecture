'use client';

import { cn } from '@/lib/utils';
import { Button } from './ui/button';
import { Label } from './ui/label';
import { Input } from './ui/input';
import { Icons } from './icons';
import { useState } from 'react';

interface UserAuthFormProps extends React.HTMLAttributes<HTMLDivElement> {}

export function UserAuthForm({ className, ...props }: UserAuthFormProps) {
  const [isLoading, setIsLoading] = useState<boolean>(false);

  async function onSubmit(event: React.SyntheticEvent) {
    event.preventDefault();
    setIsLoading(true);

    setTimeout(() => {
      setIsLoading(false);
    }, 3000);
  }

  return (
    <div className={cn('grid gap-6', className)} {...props}>
      <form onSubmit={onSubmit}>
        <div className="grid gap-2">
          <div className="grid grid-cols-2 gap-1">
            <div>
              <Label className="sr-only" htmlFor="firstname">
                Firstname
              </Label>
              <Input
                id="firstname"
                placeholder="Firstname"
                type="text"
                autoCapitalize="none"
                autoComplete="firstname"
                autoCorrect="off"
                disabled={isLoading}
              />
            </div>
            <div>
              <Label className="sr-only" htmlFor="lastname">
                Lastname
              </Label>
              <Input
                id="lastname"
                placeholder="Lastname"
                type="text"
                autoCapitalize="none"
                autoComplete="lastname"
                autoCorrect="off"
                disabled={isLoading}
              />
            </div>
          </div>
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
            Sign In with Email
          </Button>
        </div>
      </form>
    </div>
  );
}
