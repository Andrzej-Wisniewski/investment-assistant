import { create } from 'zustand';
import { persist } from 'zustand/middleware';

interface WatchlistStore {
  watchlist: string[];
  addToWatchlist: (item: string) => void;
  removeFromWatchlist: (item: string) => void;
  clearWatchlist: () => void;
}

export const useWatchlistStore = create<WatchlistStore>()(
  persist(
    (set) => ({
      watchlist: ['AAPL', 'GOOGL', 'AMZN', 'INTL', 'META'],
      addToWatchlist: (symbol) =>
        set((state) => ({
          watchlist: [...new Set([...state.watchlist, symbol.toUpperCase()])],
        })),
      removeFromWatchlist: (symbol) =>
        set((state) => ({
          watchlist: state.watchlist.filter((s) => s !== symbol.toUpperCase()),
        })),
      clearWatchlist: () => set({ watchlist: [] }),
    }),
    {
      name: 'watchlist',
    },
  ),
);
