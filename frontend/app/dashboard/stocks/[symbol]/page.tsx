'use client';

import { useStock, useRecommendation } from '@/hooks/useStock';
import { RecommendationCard } from '../../../components/RecommendationCard';
import Link from 'next/link';

export default function StockDetailPage({
  params,
}: {
  params: { symbol: string };
}) {
  const symbol = params.symbol.toUpperCase();
  const { data: stock, isLoading: stockLoading } = useStock(symbol);
  const { data: recommendation, isLoading: recLoading } =
    useRecommendation(symbol);

  if (stockLoading) {
    return <div className="text-center py-12">Ładowanie...</div>;
  }

  if (!stock) {
    return (
      <div className="text-center py-12">
        <p className="text-red-600 font-semibold mb-4">
          Nie znaleziono danych dla {symbol}
        </p>
        <Link href="/dashboard" className="text-blue-600 hover:underline">
          Powrót do dashboard
        </Link>
      </div>
    );
  }

  const change = stock.currentPrice - stock.previousClosePrice;
  const changePercent = (change / stock.previousClosePrice) * 100;

  return (
    <div>
      <Link
        href="/dashboard"
        className="text-blue-600 hover:underline mb-6 inline-block"
      >
        ← Powrót do dashboard
      </Link>

      <div className="bg-white p-8 rounded-lg shadow mb-8">
        <div className="flex justify-between items-start mb-8">
          <div>
            <h1 className="text-4xl font-bold">{symbol}</h1>
            <p className="text-gray-600">
              Ostatnia aktualizacja:{' '}
              {new Date(stock.lastUpdatedAt).toLocaleString('pl-PL')}
            </p>
          </div>
          <div
            className={`text-right text-3xl font-bold ${change >= 0 ? 'text-green-600' : 'text-red-600'}`}
          >
            ${stock.currentPrice.toFixed(2)}
            <div className="text-lg">
              {change >= 0 ? '+' : ''}
              {changePercent.toFixed(2)}%
            </div>
          </div>
        </div>

        <div className="grid grid-cols-2 md:grid-cols-4 gap-6">
          <div className="border-l-4 border-blue-500 pl-4">
            <p className="text-gray-600 text-sm">Otwarcie</p>
            <p className="text-2xl font-bold">${stock.openPrice.toFixed(2)}</p>
          </div>
          <div className="border-l-4 border-green-500 pl-4">
            <p className="text-gray-600 text-sm">Max</p>
            <p className="text-2xl font-bold">${stock.highPrice.toFixed(2)}</p>
          </div>
          <div className="border-l-4 border-red-500 pl-4">
            <p className="text-gray-600 text-sm">Min</p>
            <p className="text-2xl font-bold">${stock.lowPrice.toFixed(2)}</p>
          </div>
          <div className="border-l-4 border-purple-500 pl-4">
            <p className="text-gray-600 text-sm">Zamknięcie</p>
            <p className="text-2xl font-bold">
              ${stock.previousClosePrice.toFixed(2)}
            </p>
          </div>
        </div>
      </div>

      {recLoading ? (
        <div className="text-center py-12">Ładowanie rekomendacji...</div>
      ) : recommendation ? (
        <RecommendationCard recommendation={recommendation} />
      ) : null}
    </div>
  );
}
