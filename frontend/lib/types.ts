export interface Stock {
  symbol: string;
  currentPrice: number;
  openPrice: number;
  highPrice: number;
  lowPrice: number;
  previousClosePrice: number;
  lastUpdatedAt: string;
}

export interface RecommendationResponse {
  symbol: string;
  signal: 'BUY' | 'SELL' | 'HOLD';
  confidence: number;
  finalScore: number;
  technicalIndicators: {
    currentPrice: number;
    rsi: number;
    sma20: number;
  };
  reasoning: string;
  analyzedAt: string;
}

export interface User {
  id: string;
  email: string;
  fullName: string;
  phoneNumber?: string;
  createdAt: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
  phoneNumber?: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}
