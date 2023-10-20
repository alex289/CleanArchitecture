export type PagedResult<T> = {
  count: number;
  items: T[];
  page: number;
  pageSize: number;
};
