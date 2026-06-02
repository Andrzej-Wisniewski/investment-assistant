'use client';

import { useQuery } from '@tanstack/react-query';
import apiClient from '@/lib/api';
import { Stock, RecommendationResponse } from '@/lib/types';

export function useStock(symbol: string) {
  return useQuery({
    queryKey: ['stock', symbol],
    queryFn: async () => {
      const response = await apiClient.get<Stock>(`/stocks/${symbol}`);
      return response.data;
    },
    enabled: !!symbol,
  });
}

export function useRecommendation(symbol: string) {
  return useQuery({
    queryKey: ['recommendation', symbol],
    queryFn: async () => {
      const response = await apiClient.get<RecommendationResponse>(
        `/recommendation/${symbol}`,
      );
      return response.data;
    },
    enabled: !!symbol,
  });
}

export function useStocksList(symbols: string[]) {
  return useQuery({
    queryKey: ['stocks', symbols],
    queryFn: async () => {
      const request = symbols.map((symbol) =>
        apiClient.get<Stock>(`/stocks/${symbol}`),
      );
      const responses = await Promise.all(request);
      return responses.map((r) => r.data);
    },
    enabled: symbols.length > 0,
  });
}
