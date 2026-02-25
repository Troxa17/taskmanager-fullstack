import { apiFetch, setToken } from "./client";

type AuthResponse = {accessToken: string};

export async function login(email: string, password: string) {
    const res = await apiFetch("/api/auth/login",{
        method: "POST",
        body: JSON.stringify({email, password}),
    });
    if(!res.ok) throw new Error("Login failed");

    const data: AuthResponse = await res.json();
    setToken(data.accessToken);
}

export async function register(email: string, password: string) {
    const res = await apiFetch("/api/auth/register",{
        method: "POST",
        body: JSON.stringify({email, password}),
    });
    if(!res.ok) throw new Error("Register failed");

    const data: AuthResponse = await res.json();
    setToken(data.accessToken);
}
