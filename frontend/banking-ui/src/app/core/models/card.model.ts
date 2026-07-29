export interface Card {
  id: string;
  accountId: string;
  maskedNumber: string;
  type: string;
  status: string;
  expiryDate: string;
  dailyLimit: number;
  blockReason?: string;
}
