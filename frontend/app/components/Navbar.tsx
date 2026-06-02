'use client';

import { useAuth } from '@/hooks/useAuth';
import Link from 'next/link';
import { useRouter } from 'next/navigation';

export function Navbar() {
  const { user, logout } = useAuth();
  const router = useRouter();

  if (!user) return null;

  const handleLogout = () => {
    logout();
    router.push('/auth/login');
  };

  return (
    <nav className="bg-gradient-to-r from-blue-900 to-blue-700 text-white shadow-lg">
      <div className="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center">
        <Link href="/dashboard" className="text-2xl font-bold">
          📊 Investment Assistant
        </Link>

        <div className="flex gap-6 items-center">
          <Link href="/dashboard" className="hover:text-blue-200 transition">
            Dashboard
          </Link>
          <Link
            href="/dashboard/recommendations"
            className="hover:text-blue-200 transition"
          >
            Rekomendacje
          </Link>

          <div className="border-l border-blue-500 pl-6">
            <span className="text-sm">{user.email}</span>
            <button
              onClick={handleLogout}
              className="ml-4 bg-red-600 hover:bg-red-700 px-4 py-2 rounded transition"
            >
              Wyloguj
            </button>
          </div>
        </div>
      </div>
    </nav>
  );
}
