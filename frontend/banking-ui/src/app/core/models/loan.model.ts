export interface Loan {
  id: string;
  type: string;
  principalAmount: number;
  interestRatePercent: number;
  termMonths: number;
  status: string;
  appliedAt: string;
  rejectionReason?: string;
}

export interface LoanApplicationRequest {
  customerId: string;
  loanType: string;
  principalAmount: number;
  termMonths: number;
  annualIncomeEstimate: number;
}
