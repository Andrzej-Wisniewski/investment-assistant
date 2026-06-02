'use client';

import { useAuth } from '@/hooks/useAuth';
import { useRouter } from 'next/navigation';
import { useEffect } from 'react';
import Link from 'next/link';

export default function Home() {
  const { user, loading } = useAuth();
  const router = useRouter();

  useEffect(() => {
    if (!loading && user) {
      router.push('/dashboard');
    }
  }, [user, loading, router]);

  if (loading) {
    return (
      <div className="flex items-center justify-center h-screen">
        Ładowanie...
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-900 via-blue-700 to-blue-500">
      <nav className="bg-black/30 backdrop-blur border-b border-white/10">
        <div className="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center">
          <h1 className="text-2xl font-bold text-white">
            📊 Investment Assistant
          </h1>
          <div className="flex gap-4">
            <Link
              href="/auth/login"
              className="px-6 py-2 text-white hover:bg-white/10 rounded-lg transition"
            >
              Zaloguj się
            </Link>
            <Link
              href="/auth/register"
              className="px-6 py-2 bg-white text-blue-900 font-semibold rounded-lg hover:bg-gray-100 transition"
            >
              Zarejestruj się
            </Link>
          </div>
        </div>
      </nav>

      <div className="max-w-7xl mx-auto px-6 py-20">
        <div className="text-center text-white mb-12">
          <h2 className="text-5xl font-bold mb-4">
            Inteligentne rekomendacje inwestycyjne
          </h2>
          <p className="text-xl text-blue-100 mb-8">
            Analiza techniczna w czasie rzeczywistym. Wskaźniki RSI i SMA20.
            Sygnały kupna/sprzedaży oparte na algorytmie punktacji ważonej.
          </p>
          <Link
            href="/auth/register"
            className="inline-block px-8 py-4 bg-white text-blue-900 font-bold rounded-lg hover:bg-gray-100 transition text-lg"
          >
            Rozpocznij za darmo
          </Link>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mt-16">
          <div className="bg-white/10 backdrop-blur border border-white/20 p-6 rounded-lg text-white">
            <h3 className="text-xl font-bold mb-3">📈 Analiza Real-Time</h3>
            <p className="text-blue-100">
              Pobieramy dane z Alpha Vantage. Zawsze masz aktualne ceny akcji.
            </p>
          </div>

          <div className="bg-white/10 backdrop-blur border border-white/20 p-6 rounded-lg text-white">
            <h3 className="text-xl font-bold mb-3">🧠 Inteligentny Algorytm</h3>
            <p className="text-blue-100">
              System punktacji ważonej. 60% RSI + 40% SMA20 = precyzyjny sygnał.
            </p>
          </div>

          <div className="bg-white/10 backdrop-blur border border-white/20 p-6 rounded-lg text-white">
            <h3 className="text-xl font-bold mb-3">⚡ Szybkie Decyzje</h3>
            <p className="text-blue-100">
              Rekomendacje BUY/SELL/HOLD z poziomem pewności (0-100%).
            </p>
          </div>
        </div>

        <div className="bg-white/10 backdrop-blur border border-white/20 p-8 rounded-lg text-white mt-16">
          <h3 className="text-2xl font-bold mb-4">Jak to działa?</h3>
          <div className="space-y-3">
            <p>
              1️⃣ <strong>Rejestracja</strong> — Załóż bezpłatne konto
            </p>
            <p>
              2️⃣ <strong>Obserwacja</strong> — Dodaj akcje do listy obserwacji
            </p>
            <p>
              3️⃣ <strong>Analiza</strong> — Otrzymaj rekomendacje oparte na
              danych
            </p>
            <p>
              4️⃣ <strong>Inwestycja</strong> — Podejmij świadomą decyzję
            </p>
          </div>
        </div>
      </div>

      <footer className="border-t border-white/10 mt-20 py-6 text-center text-blue-100">
        <p>© 2026 Investment Assistant. Narzędzie edukacyjne.</p>
      </footer>
    </div>
  );
}
