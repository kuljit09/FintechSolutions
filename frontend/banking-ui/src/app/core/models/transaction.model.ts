export interface Transaction {
  id: string;
  accountId: string;
  type: string;
  status: string;
  amount: number;
  currency: string;
  merchant?: string;
  description?: string;
  failureReason?: string;
  timestamp: string;
}

export interface TransactionFailureExplanation {
  transactionId: string;
  failed: boolean;
  explanation: string;
}

export interface DisputeEligibility {
  transactionId: string;
  eligible: boolean;
  reason: string;
}
