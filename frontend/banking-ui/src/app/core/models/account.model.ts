export interface Account {
  id: string;
  accountNumber: string;
  type: string;
  status: string;
  balance: number;
  overdraftLimit: number;
  currency: string;
}
