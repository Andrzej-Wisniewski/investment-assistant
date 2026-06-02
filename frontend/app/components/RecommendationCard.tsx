'use client';

import { RecommendationResponse } from '@/lib/types';

interface RecommendationCardProps {
  recommendation: RecommendationResponse;
}

export function RecommendationCard({
  recommendation,
}: RecommendationCardProps) {
  const signalColors = {
    BUY: 'bg-green-100 text-green-800 border-green-300',
    SELL: 'bg-red-100 text-red-800 border-red-300',
    HOLD: 'bg-yellow-100 text-yellow-800 border-yellow-300',
  };

  const signalEmoji = {
    BUY: '📈',
    SELL: '📉',
    HOLD: '➡️',
  };

  const color = signalColors[recommendation.signal];

  return (
    <div className="bg-white p-6 rounded-lg shadow">
      <div className="flex justify-between items-start mb-4">
        <h3 className="text-xl font-bold">{recommendation.symbol}</h3>
        <div className={`px-4 py-2 rounded-lg border-2 font-bold ${color}`}>
          {signalEmoji[recommendation.signal]} {recommendation.signal}
        </div>
      </div>

      <div className="mb-4">
        <div className="flex justify-between text-sm mb-2">
          <span className="text-gray-600">Pewność:</span>
          <span className="font-semibold">
            {(recommendation.confidence * 100).toFixed(0)}%
          </span>
        </div>
        <div className="w-full bg-gray-200 rounded-full h-2">
          <div
            className={`h-2 rounded-full transition-all ${
              recommendation.signal === 'BUY'
                ? 'bg-green-500'
                : recommendation.signal === 'SELL'
                  ? 'bg-red-500'
                  : 'bg-yellow-500'
            }`}
            style={{ width: `${recommendation.confidence * 100}%` }}
          />
        </div>
      </div>

      <div className="grid grid-cols-3 gap-4 mb-4">
        <div className="text-center">
          <p className="text-gray-500 text-sm">Cena</p>
          <p className="font-bold">
            ${recommendation.technicalIndicators.currentPrice.toFixed(2)}
          </p>
        </div>
        <div className="text-center">
          <p className="text-gray-500 text-sm">RSI</p>
          <p className="font-bold">
            {recommendation.technicalIndicators.rsi.toFixed(1)}
          </p>
        </div>
        <div className="text-center">
          <p className="text-gray-500 text-sm">SMA20</p>
          <p className="font-bold">
            ${recommendation.technicalIndicators.sma20.toFixed(2)}
          </p>
        </div>
      </div>

      <div className="bg-blue-50 p-4 rounded border border-blue-200">
        <p className="text-sm text-gray-700">{recommendation.reasoning}</p>
      </div>

      <p className="text-xs text-gray-500 mt-4 text-right">
        Analiza: {new Date(recommendation.analyzedAt).toLocaleString('pl-PL')}
      </p>
    </div>
  );
}
