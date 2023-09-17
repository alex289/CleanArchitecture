export type ApiResponse<T> = {
    success: boolean;
    errors?: string[];
    detailedErrors?: DetailedError[];
    data?: T;
}

type DetailedError = {
    code: string;
    data: object;
}