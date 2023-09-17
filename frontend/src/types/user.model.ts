import { UserRole } from './user-role.enum';
import { UserStatus } from './user-status.enum';

export type UserModel = {
  id: string;
  tenantId: string;
  email: string;
  firstName: string;
  lastName: string;
  role: UserRole;
  status: UserStatus;
};
