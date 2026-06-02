'use client';

import { Stock } from '@/lib/types';
import Link from 'next/link';

interface StockCardProps {
  stock: Stock;
}

export function StockCard({ stock }: StockCardProps) {
  const change = stock.currentPrice - stock.previousClosePrice;
  const changePercent = (change / stock.previousClosePrice) * 100;
  const isPositive = change > 0;

  return (
    <Link href={`/dashboard/stocks/${stock.symbol}`}>
      <div className="bg-white p-6 rounded-lg shadow hover:shadow-lg transition cursor-pointer">
        <div className="flex justify-between items-start mb-4">
          <div>
            <h3 className="text-xl font-bold">{stock.symbol}</h3>
            <p className="text-sm text-gray-500">
              Ostatnia aktualizacja:{' '}
              {new Date(stock.lastUpdatedAt).toLocaleTimeString('pl-PL')}
            </p>
          </div>
          <div
            className={`text-right ${isPositive ? 'text-green-600' : 'text-red-600'}`}
          >
            <div className="text-2xl font-bold">
              ${stock.currentPrice.toFixed(2)}
            </div>
            <div className="text-sm">
              {isPositive ? '+' : ''}
              {changePercent.toFixed(2)}%
            </div>
          </div>
        </div>

        <div className="grid grid-cols-2 gap-4 text-sm">
          <div>
            <span className="text-gray-500">Otwarcie:</span>
            <p className="font-semibold">${stock.openPrice.toFixed(2)}</p>
          </div>
          <div>
            <span className="text-gray-500">Zamknięcie:</span>
            <p className="font-semibold">
              ${stock.previousClosePrice.toFixed(2)}
            </p>
          </div>
          <div>
            <span className="text-gray-500">Min:</span>
            <p className="font-semibold">${stock.lowPrice.toFixed(2)}</p>
          </div>
          <div>
            <span className="text-gray-500">Max:</span>
            <p className="font-semibold">${stock.highPrice.toFixed(2)}</p>
          </div>
        </div>
      </div>
    </Link>
  );
}
