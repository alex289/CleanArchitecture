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
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';

import type { ApiResponse } from '@/types/api-response';
import type { TenantModel } from '@/types/tenant.model';

export default function UpsertTenantDialog({
  tenantData,
  isUpdating,
}: {
  tenantData?: TenantModel;
  isUpdating: boolean;
}) {
  const router = useRouter();

  async function submit(event: React.SyntheticEvent) {
    event.preventDefault();

    if (isUpdating) {
      const res = await fetch(
        `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/v1/tenant`,
        {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('auth_token')}`,
          },
          body: JSON.stringify({
            id: tenantData?.id,
            name: (event.target as any).name.value,
          }),
        },
      );

      if (res.ok) {
        window.location.reload();
      }
    } else {
      const res = await fetch(
        `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/v1/tenant`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('auth_token')}`,
          },
          body: JSON.stringify({
            name: (event.target as any).name.value,
          }),
        },
      );

      if (res.ok) {
        const body = (await res.json()) as ApiResponse<string>;
        router.push(`/dashboard/tenant/${body.data}`);
      }
    }
  }
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="outline">
          {isUpdating ? 'Edit tenant' : 'Add tenant'}
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <form onSubmit={submit}>
          <DialogHeader>
            <DialogTitle>
              {isUpdating ? 'Edit tenant' : 'Add tenant'}
            </DialogTitle>
            <DialogDescription>
              Make changes to the tenant here. Click save when you are done.
            </DialogDescription>
          </DialogHeader>
          <div className="grid gap-4 py-4">
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="name" className="text-right">
                Name
              </Label>
              <Input
                id="name"
                defaultValue={tenantData?.name}
                className="col-span-3"
              />
            </div>
          </div>
          <DialogFooter>
            <Button type="submit">Save changes</Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
