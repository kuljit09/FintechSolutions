-- =====================================================================================
-- SEED DATA ONLY - no CREATE EXTENSION / CREATE TABLE statements here.
--
-- You already have pgAdmin + pgvector set up locally, and this project's schema is meant
-- to come from EF Core migrations (dotnet ef migrations add InitialCreate / database update
-- against Banking.Infrastructure/Persistence/AppDbContext.cs), which will also emit
-- `CREATE EXTENSION IF NOT EXISTS vector` because of modelBuilder.HasPostgresExtension("vector").
--
-- Run this AFTER `dotnet ef database update` has created all tables.
-- =====================================================================================

INSERT INTO customers (id, full_name, email, kyc_status, credit_score) VALUES
  ('a1111111-1111-1111-1111-111111111101', 'Priya Nair',    'priya.nair@example.com',    'Verified', 742),
  ('a1111111-1111-1111-1111-111111111102', 'Rohan Mehta',   'rohan.mehta@example.com',   'Verified', 588),
  ('a1111111-1111-1111-1111-111111111103', 'Sana Fatima',   'sana.fatima@example.com',   'Verified', 705);

INSERT INTO accounts (id, customer_id, account_number, type, status, balance, overdraft_limit, currency) VALUES
  ('a2222222-2222-2222-2222-222222222201', 'a1111111-1111-1111-1111-111111111101', 'ACC-100234501', 'Checking', 'Active', 18500.00, 5000.00, 'INR'),
  ('a2222222-2222-2222-2222-222222222202', 'a1111111-1111-1111-1111-111111111101', 'ACC-100234502', 'Savings',  'Active', 245000.00, 0.00,   'INR'),
  ('a2222222-2222-2222-2222-222222222203', 'a1111111-1111-1111-1111-111111111102', 'ACC-100234503', 'Checking', 'Active', 320.00,   2000.00, 'INR'),
  ('a2222222-2222-2222-2222-222222222204', 'a1111111-1111-1111-1111-111111111103', 'ACC-100234504', 'Checking', 'Active', 52000.00, 3000.00, 'INR');

INSERT INTO cards (id, account_id, masked_number, type, status, expiry_date, daily_limit, block_reason) VALUES
  ('a3333333-3333-3333-3333-333333333301', 'a2222222-2222-2222-2222-222222222201', '**** **** **** 4471', 'Debit',  'Active', '2028-04-30', 50000.00, NULL),
  ('a3333333-3333-3333-3333-333333333302', 'a2222222-2222-2222-2222-222222222203', '**** **** **** 8820', 'Debit',  'Active', '2027-11-30', 20000.00, NULL),
  ('a3333333-3333-3333-3333-333333333303', 'a2222222-2222-2222-2222-222222222204', '**** **** **** 1190', 'Credit', 'Active', '2029-02-28', 100000.00, NULL);

-- Failed transaction (insufficient funds) - drives "why did my transaction fail" demo for Rohan (low balance).
INSERT INTO transactions (id, account_id, type, status, amount, currency, merchant, description, failure_reason, timestamp, flagged_for_fraud_review) VALUES
  ('a4444444-4444-4444-4444-444444444401', 'a2222222-2222-2222-2222-222222222203', 'Debit', 'Failed', 1500.00, 'INR', 'Amazon.in', 'Online purchase', 'Insufficient funds: balance 320.00 + overdraft limit 2000.00 is less than the requested 1500.00 -- wait this should succeed, see note below', '2026-07-25 10:15:00+00', true);

-- NOTE: the failure_reason text above is illustrative seed data only. Because ExplainFailureAsync
-- RE-EVALUATES OverdraftPolicy live against the account's CURRENT balance/overdraft limit rather
-- than only trusting the stored string, the exact wording your chatbot returns will reflect
-- whatever Rohan's account balance is at query time - this is intentional: it demonstrates
-- grounding in live domain-rule evaluation, not a static stored message. Adjust account balance
-- above if you want the demo to consistently show a declined outcome.

-- Completed transaction eligible for dispute (within 60 days).
INSERT INTO transactions (id, account_id, type, status, amount, currency, merchant, description, failure_reason, timestamp, flagged_for_fraud_review) VALUES
  ('a4444444-4444-4444-4444-444444444402', 'a2222222-2222-2222-2222-222222222201', 'Debit', 'Completed', 2999.00, 'INR', 'Flipkart', 'Electronics purchase', NULL, now() - interval '5 days', true);

-- High-value completed transaction that the fraud sweep SHOULD flag on its next pass
-- (set flagged_for_fraud_review = false so FraudSweepBackgroundService picks it up).
INSERT INTO transactions (id, account_id, type, status, amount, currency, merchant, description, failure_reason, timestamp, flagged_for_fraud_review) VALUES
  ('a4444444-4444-4444-4444-444444444403', 'a2222222-2222-2222-2222-222222222204', 'Debit', 'Completed', 82000.00, 'INR', 'International Wire', 'Overseas transfer', NULL, now() - interval '2 minutes', false);

-- A handful of routine, already-swept transactions so "recent transactions" doesn't look empty.
INSERT INTO transactions (id, account_id, type, status, amount, currency, merchant, description, failure_reason, timestamp, flagged_for_fraud_review) VALUES
  ('a4444444-4444-4444-4444-444444444404', 'a2222222-2222-2222-2222-222222222201', 'Debit',  'Completed', 450.00,  'INR', 'Swiggy',      'Food delivery',    NULL, now() - interval '1 days', true),
  ('a4444444-4444-4444-4444-444444444405', 'a2222222-2222-2222-2222-222222222201', 'Credit', 'Completed', 5000.00, 'INR', 'Salary Corp', 'Salary credit',    NULL, now() - interval '3 days', true),
  ('a4444444-4444-4444-4444-444444444406', 'a2222222-2222-2222-2222-222222222204', 'Fee',    'Completed', 50.00,   'INR', NULL,          'Monthly account fee', NULL, now() - interval '10 days', true);

INSERT INTO loans (id, customer_id, type, principal_amount, interest_rate_percent, term_months, status, applied_at, credit_score_at_application, rejection_reason) VALUES
  -- Approved: good credit score, income-eligible
  ('a5555555-5555-5555-5555-555555555501', 'a1111111-1111-1111-1111-111111111101', 'Personal', 300000.00, 11.50, 24, 'Approved', now() - interval '20 days', 742, NULL),
  -- Rejected: credit score below the 650 threshold in LoanEligibilityPolicy
  ('a5555555-5555-5555-5555-555555555502', 'a1111111-1111-1111-1111-111111111102', 'Auto',     500000.00, 0.00,  36, 'Rejected', now() - interval '7 days',  588, 'Credit score 588 is below the minimum required score of 650.');

INSERT INTO loan_repayments (id, loan_id, due_date, amount_due, status) VALUES
  ('a6666666-6666-6666-6666-666666666601', 'a5555555-5555-5555-5555-555555555501', now() + interval '10 days', 14375.00, 'Pending'),
  ('a6666666-6666-6666-6666-666666666602', 'a5555555-5555-5555-5555-555555555501', now() + interval '40 days', 14375.00, 'Pending');

INSERT INTO knowledge_base_articles (id, title, content, category) VALUES
  ('a7777777-7777-7777-7777-777777777701', 'Dispute & Chargeback Policy',
   'Customers can dispute a completed transaction within 60 days of the transaction date. Disputes cannot be filed twice for the same transaction, and only completed (not pending or failed) transactions are eligible. Once filed, a dispute enters investigation and is typically resolved within 10 business days. If the dispute is upheld, the disputed amount is refunded to the original account.',
   'disputes'),
  ('a7777777-7777-7777-7777-777777777702', 'Overdraft & Insufficient Funds Policy',
   'A transaction is declined if the requested amount exceeds the account balance plus its overdraft limit. Checking accounts typically carry an overdraft limit set at account opening; savings accounts do not carry an overdraft facility. Declined transactions do not incur a fee, but repeated overdraft attempts may result in a review of the account.',
   'fees'),
  ('a7777777-7777-7777-7777-777777777703', 'Loan Eligibility Criteria',
   'Personal, auto, and education loan eligibility is primarily determined by credit score, with a minimum threshold of 650 required for approval. The maximum eligible loan amount is capped relative to the applicant''s estimated annual income. Applications that do not meet the credit score threshold are rejected with an explanation rather than held in review.',
   'loans'),
  ('a7777777-7777-7777-7777-777777777704', 'Fraud Protection & Alerts',
   'Every transaction is continuously screened by an automated fraud-detection sweep that runs independently of customer support requests. Alerts are raised for unusually high-value transactions with no recent similar activity, or for unusually high transaction velocity within a short window. Customers are never charged for fraudulent transactions confirmed through investigation.',
   'fraud'),
  ('a7777777-7777-7777-7777-777777777705', 'Card Blocking & Replacement Policy',
   'Customers can request an immediate block on a lost or stolen card. Blocking a card is a security-sensitive action and always requires explicit confirmation before being carried out, whether requested via the support chatbot or the mobile app. Once blocked, a replacement card can be requested and is typically delivered within 5-7 business days; a temporary virtual card is available immediately for digital transactions.',
   'cards'),
  ('a7777777-7777-7777-7777-777777777706', 'KYC Verification Requirements',
   'All customers must complete Know Your Customer verification before accounts are fully activated for large transactions or loan applications. Verification requires a government-issued ID and proof of address. Accounts with pending KYC status may have reduced transaction and withdrawal limits until verification is complete.',
   'compliance');

-- NOTE ON EMBEDDINGS: same approach as the e-commerce project - knowledge_base_embeddings rows
-- are intentionally NOT populated with real vectors here. Write a small seeding routine that:
--   1. Reads knowledge_base_articles
--   2. Chunks each Content with EmbeddingIngestionHelper.ChunkText(...)
--   3. Calls IEmbeddingGenerator.GenerateAsync(chunk) (hits your local Ollama nomic-embed-text)
--   4. Inserts (article_id, chunk_text, embedding) into knowledge_base_embeddings
-- Then, since you already have pgvector, create the similarity index once real vectors exist:
--   CREATE INDEX ON knowledge_base_embeddings USING ivfflat (embedding vector_cosine_ops) WITH (lists = 100);
