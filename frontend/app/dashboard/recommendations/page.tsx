'use client';

import { useWatchlistStore } from '@/lib/store';
import { useRecommendation } from '@/hooks/useStock';
import { RecommendationCard } from '../../components/RecommendationCard';

export default function RecommendationsPage() {
  const watchlist = useWatchlistStore((state) => state.watchlist);

  return (
    <div>
      <h1 className="text-3xl font-bold mb-8">Rekomendacje inwestycyjne 💡</h1>

      {watchlist.length === 0 ? (
        <div className="bg-blue-50 border border-blue-200 p-8 rounded-lg text-center">
          <p className="text-gray-600 text-lg">
            Brak akcji do analizy. Dodaj akcje na dashboard!
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          {watchlist.map((symbol) => (
            <RecommendationItem key={symbol} symbol={symbol} />
          ))}
        </div>
      )}
    </div>
  );
}

function RecommendationItem({ symbol }: { symbol: string }) {
  const { data: recommendation, isLoading, error } = useRecommendation(symbol);

  if (isLoading) {
    return (
      <div className="bg-white p-6 rounded-lg shadow animate-pulse">
        <div className="h-8 bg-gray-200 rounded w-1/3 mb-4"></div>
        <div className="h-4 bg-gray-200 rounded mb-2"></div>
        <div className="h-4 bg-gray-200 rounded w-2/3"></div>
      </div>
    );
  }

  if (error || !recommendation) {
    return (
      <div className="bg-red-50 border border-red-200 p-6 rounded-lg">
        <p className="text-red-800 font-semibold">{symbol}</p>
        <p className="text-red-600 text-sm">Błąd ładowania rekomendacji</p>
      </div>
    );
  }

  return <RecommendationCard recommendation={recommendation} />;
}
