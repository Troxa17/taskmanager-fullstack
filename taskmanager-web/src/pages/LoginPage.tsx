import { useState } from "react";
import { login, register } from "../api/auth";
import type { ApiError } from "../api/client";

function isValidEmail(email: string) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

export default function LoginPage() {
  const [email, setEmail] = useState("a@a.com");
  const [password, setPassword] = useState("123456");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  async function doAuth(fn: () => Promise<void>) {
    setError(null);

    if (!isValidEmail(email)) {
      setError("Email error");
      return;
    }

    if (password.length < 6) {
      setError("Password need be over 6 symbols");
      return;
    }

    setLoading(true);
    try {
      await fn();
      window.location.href = "/";
    } catch (e) {
      const err = e as ApiError;
      setError(`${err.status}: ${err.message}`);
    } finally {
      setLoading(false);
    }
  }

 return (
  <div className="min-h-screen bg-slate-950 text-slate-100 flex items-center justify-center p-6">
    <div className="w-full max-w-md bg-slate-900/60 border border-slate-800 rounded-2xl p-8 shadow-xl">

      <h1 className="text-2xl font-bold mb-2">TaskManager</h1>
      <p className="text-slate-400 mb-6">Sign in / Sign up</p>

      <div className="space-y-4">
        <input
          className="w-full rounded-xl bg-slate-800 border border-slate-700 px-4 py-2 text-slate-100 outline-none focus:ring-2 focus:ring-indigo-500"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          disabled={loading}
        />

        <input
          type="password"
          className="w-full rounded-xl bg-slate-800 border border-slate-700 px-4 py-2 text-slate-100 outline-none focus:ring-2 focus:ring-indigo-500"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          disabled={loading}
        />

        {error && <div className="text-red-400 text-sm">{error}</div>}

        <div className="flex gap-3 pt-2">
          <button
            className="flex-1 bg-indigo-600 hover:bg-indigo-500 rounded-xl py-2 font-medium transition"
            disabled={loading}
            onClick={() => doAuth(() => login(email, password))}
          >
            {loading ? "..." : "Login"}
          </button>

          <button
            className="flex-1 bg-slate-800 border border-slate-700 hover:bg-slate-700 rounded-xl py-2 font-medium transition"
            disabled={loading}
            onClick={() => doAuth(() => register(email, password))}
          >
            {loading ? "..." : "Register"}
          </button>
        </div>
      </div>

    </div>
  </div>
);
}