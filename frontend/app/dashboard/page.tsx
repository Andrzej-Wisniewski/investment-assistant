'use client';

import { useAuth } from '@/hooks/useAuth';
import { useWatchlistStore } from '@/lib/store';
import { useStocksList } from '@/hooks/useStock';
import { StockCard } from '../components/StockCard';
import { useState } from 'react';

export default function DashboardPage() {
  const { user } = useAuth();
  const watchlist = useWatchlistStore((state) => state.watchlist);
  const addToWatchlist = useWatchlistStore((state) => state.addToWatchlist);
  const { data: stocks, isLoading } = useStocksList(watchlist);
  const [newSymbol, setNewSymbol] = useState('');

  const handleAddStock = (e: React.FormEvent) => {
    e.preventDefault();
    if (newSymbol.trim()) {
      addToWatchlist(newSymbol.toUpperCase());
      setNewSymbol('');
    }
  };

  return (
    <div>
      <h1 className="text-3xl font-bold mb-2">Witaj, {user?.fullName}! 👋</h1>
      <p className="text-gray-600 mb-8">
        Tutaj możesz śledzić swoje ulubione akcje
      </p>

      <div className="bg-white p-6 rounded-lg shadow mb-8">
        <h2 className="text-xl font-bold mb-4">Dodaj akcję do obserwacji</h2>
        <form onSubmit={handleAddStock} className="flex gap-2">
          <input
            type="text"
            placeholder="np. AAPL, GOOGL, MSFT..."
            value={newSymbol}
            onChange={(e) => setNewSymbol(e.target.value.toUpperCase())}
            maxLength={5}
            className="flex-1 px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <button
            type="submit"
            className="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition"
          >
            Dodaj
          </button>
        </form>
      </div>

      {isLoading ? (
        <div className="flex items-center justify-center h-64">
          <p className="text-gray-500 text-lg">Ładowanie danych...</p>
        </div>
      ) : stocks && stocks.length > 0 ? (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {stocks.map((stock) => (
            <StockCard key={stock.symbol} stock={stock} />
          ))}
        </div>
      ) : (
        <div className="bg-blue-50 border border-blue-200 p-8 rounded-lg text-center">
          <p className="text-gray-600 text-lg">
            Brak akcji na liście obserwacji. Dodaj swoją pierwszą akcję!
          </p>
        </div>
      )}
    </div>
  );
}
