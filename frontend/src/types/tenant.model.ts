import { UserModel } from './user.model';

export type TenantModel = {
  id: string;
  name: string;
  users: UserModel[];
};
