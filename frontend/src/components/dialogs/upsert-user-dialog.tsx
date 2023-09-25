import { useRouter } from 'next/navigation';

import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog';
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { ApiResponse } from '@/types/api-response';

import type { UserModel } from '@/types/user.model';
import { useState } from 'react';

export default function UpsertUserDialog({
  userData,
  isUpdating,
  tenantId,
}: {
  userData?: UserModel;
  isUpdating: boolean;
  tenantId?: string;
}) {
  const [role, setRole] = useState(userData?.role);
  const router = useRouter();

  async function submit(event: React.SyntheticEvent) {
    event.preventDefault();

    if (isUpdating) {
      const res = await fetch(
        `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/v1/user`,
        {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('auth_token')}`,
          },
          body: JSON.stringify({
            id: userData?.id,
            tenantId: userData?.tenantId,
            email: (event.target as any).email.value,
            firstName: (event.target as any).firstName.value,
            lastName: (event.target as any).lastName.value,
            role: role,
          }),
        },
      );

      if (res.ok) {
        window.location.reload();
      }
    } else {
      const res = await fetch(
        `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/v1/user`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('auth_token')}`,
          },
          body: JSON.stringify({
            tenantId: tenantId,
            email: (event.target as any).email.value,
            firstName: (event.target as any).firstName.value,
            lastName: (event.target as any).lastName.value,
            password: (event.target as any).password.value,
          }),
        },
      );

      if (res.ok) {
        const body = (await res.json()) as ApiResponse<string>;
        router.push(`/dashboard/user/${body.data}`);
      }
    }
  }
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="outline">
          {isUpdating ? 'Edit user' : 'Add user'}
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <form onSubmit={submit}>
          <DialogHeader>
            <DialogTitle>{isUpdating ? 'Edit user' : 'Add user'}</DialogTitle>
            <DialogDescription>
              Make changes to the user here. Click save when you are done.
            </DialogDescription>
          </DialogHeader>
          <div className="grid gap-4 py-4">
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="firstName" className="text-right">
                First name
              </Label>
              <Input
                id="firstName"
                defaultValue={userData?.firstName}
                className="col-span-3"
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="lastName" className="text-right">
                Last name
              </Label>
              <Input
                id="lastName"
                defaultValue={userData?.lastName}
                className="col-span-3"
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="email" className="text-right">
                Email
              </Label>
              <Input
                id="email"
                defaultValue={userData?.email}
                className="col-span-3"
              />
            </div>
            {!isUpdating && (
              <div className="grid grid-cols-4 items-center gap-4">
                <Label htmlFor="password" className="text-right">
                  Password
                </Label>
                <Input
                  id="password"
                  type="password"
                  defaultValue={userData?.email}
                  className="col-span-3"
                />
              </div>
            )}
            {isUpdating && (
              <div className="grid grid-cols-4 items-center gap-4">
                <Label htmlFor="role" className="text-right">
                  Role
                </Label>
                <div className="col-span-3">
                  <Select
                    defaultValue={userData?.role.toString()}
                    onValueChange={(value) => setRole(Number(value))}>
                    <SelectTrigger>
                      <SelectValue placeholder="Select a role" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectGroup>
                        <SelectItem value="0">Admin</SelectItem>
                        <SelectItem value="1">User</SelectItem>
                      </SelectGroup>
                    </SelectContent>
                  </Select>
                </div>
              </div>
            )}
          </div>
          <DialogFooter>
            <Button type="submit">Save changes</Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
