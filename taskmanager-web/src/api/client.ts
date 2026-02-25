const API_BASE = "http://localhost:5051";

export function getToken(): string | null {
    return localStorage.getItem("accessToken");
}

export function setToken(token: string) {
    return localStorage.setItem("accessToken",token);
}

export function clearToken() {
    return localStorage.removeItem("accessToken");
}

export type ApiError = {
    status: number;
    message: string;
};

async function tryReadErrorMessage(res: Response): Promise<string> {
    
    const contentType = res.headers.get("content-type") ?? "";
    if(contentType.includes("application/json")) {
        try {
            const data = await res.json();
            if(typeof data?.title === "string") return data.title;
            if(typeof data?.message === "string") return data.message;

            if(data?.errors && typeof data.errors === "object") {
                const all: string[] = [];
                for(const key of Object.keys(data.errors)) {
                    const arr = data.errors[key];
                    if(Array.isArray(arr)) all.push(`${key}: ${arr.join(", ")}`);
                }
                if(all.length > 0) return all.join(" | ");
            }
            return JSON.stringify(data);
        }catch{}
    }
    try {
        const text = await res.text();
        return text || res.statusText || "Request failed";
    } catch{
        return res.statusText || "Request failed"
    }
}



export async function apiFetch(path: string, options?: RequestInit) {
    const token = getToken();

   const headers: Record<string, string> = {
        "Content-Type": "application/json",
        ...(options?.headers as Record<string, string> ?? {})
    }
    if(token) {
        headers["Authorization"] = `Bearer ${token}`;  
    }

    let res: Response;
    try {
        res = await fetch(`${API_BASE}${path}`,{...options, headers,});
    }catch{
        throw {status: 0, message: "Network error: cannot reach API"} as ApiError;
    }
    if(res.status === 401) {
        clearToken();
        throw {status: res.status, message: "Unauthorized"} as ApiError;
    }
    if(!res.ok) {
        const msg = await tryReadErrorMessage(res);
        throw {status: res.status, message: msg} as ApiError;
    }
    return res;
} 