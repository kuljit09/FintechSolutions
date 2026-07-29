export interface ChatRequest {
  customerId: string;
  message: string;
  accountId?: string;
  conversationId?: string;
}

export interface ChatResponse {
  conversationId: string;
  answer: string;
  sourcesUsed: string[];
  toolsInvoked: string[];
  suggestedNextActions: string[];
  confidence: string;
  requiresHumanConfirmation: boolean;
}

export interface ChatMessage {
  role: 'user' | 'assistant';
  text: string;
  sourcesUsed?: string[];
  toolsInvoked?: string[];
  requiresConfirmation?: boolean;
}
