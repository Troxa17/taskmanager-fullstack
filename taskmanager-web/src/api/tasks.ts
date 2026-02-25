import { apiFetch } from "./client";

export type TaskResponse = {
    id: number,
    title: string,
    isCompleted: boolean,
    createdAtUtc: string
};

export async function getTasks(): Promise<TaskResponse[]> {
    const res = await apiFetch("/api/task");
    if(!res.ok) throw new Error("Failed to load tasks");
    return res.json();
}

export async function createTask(title: string): Promise<TaskResponse> {
    const res = await apiFetch("/api/task",{method: "POST", body: JSON.stringify({title})});

    if(!res.ok) throw new Error("Failed to create task");
    return res.json();
}

export async function updateTask(id: number, payload: {title: string, isCompleted: boolean}):Promise<void>{
    await apiFetch(`/api/task/${id}`, {method: "PUT", body: JSON.stringify(payload),});
}

export async function deleteTask(id: number): Promise<void>{
    await apiFetch(`/api/task/${id}`, {method: "DELETE"});
}