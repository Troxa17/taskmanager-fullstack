import { useEffect, useMemo, useState } from "react";
import { createTask, getTasks, updateTask, deleteTask, type TaskResponse } from "../api/tasks";
import { clearToken } from "../api/client";
import type { ApiError } from "../api/client";

export default function TasksPage() {
  const [tasks, setTasks] = useState<TaskResponse[]>([]);
  const [title, setTitle] = useState("");
  const [loading, setLoading] = useState(true);
  const [mutating, setMutating] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const remaining = useMemo(
    () => tasks.filter((t) => !t.isCompleted).length,
    [tasks]
  );

  async function load() {
    setError(null);
    setLoading(true);
    try {
      const data = await getTasks();
      setTasks(data);
    } catch (e) {
      const err = e as ApiError;
      if (err.status === 401) {
        clearToken();
        window.location.href = "/login";
        return;
      }
      setError(`${err.status}: ${err.message}`);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    load();
  }, []);

  async function addTask() {
    const trimmed = title.trim();
    if (!trimmed) return;

    setError(null);
    setMutating(true);
    try {
      const t = await createTask(trimmed);
      setTasks([t, ...tasks]);
      setTitle("");
    } catch (e) {
      const err = e as ApiError;
      if (err.status === 401) {
        clearToken();
        window.location.href = "/login";
        return;
      }
      setError(`${err.status}: ${err.message}`);
    } finally {
      setMutating(false);
    }
  }

  async function toggleTask(t: TaskResponse) {
    setError(null);
    setMutating(true);
    try{
      await updateTask(t.id, {title: t.title, isCompleted: !t.isCompleted});
      setTasks(prev =>prev.map(x => x.id === t.id ? {...x, isCompleted: !x.isCompleted}: x));
    }catch (e){
      const err = e as ApiError;
      if(err.status === 401) {
        clearToken();
        window.location.href = "/login";
        return;
      }
      setError(`${err.status}: ${err.message}`);
    }finally{setMutating(false);}
  }

  async function removeTask(id: number){
    setError(null);
    setMutating(true);
    try{
      await deleteTask(id);
      setTasks(prev => prev.filter(x=> x.id !== id));
    }catch (e){
      const err = e as ApiError;
      if(err.status === 401){
        clearToken();
        window.location.href = "/login";
        return;
      }
      setError(`${err.status}: ${err.message}`);
    }finally{setMutating(false);}
  }

  function logout() {
    clearToken();
    window.location.href = "/login";
  }

  return (
    <div className="app-page">
      <div className="container">
        <header className="flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-semibold">My Tasks</h1>
            <p className="muted mt-1">
              Left: <span className="text-slate-200 font-medium">{remaining}</span>
            </p>
          </div>
          

          <div className="flex gap-2">
            <button className="btn btn-secondary" onClick={load} disabled={loading}>
              Refresh
            </button>
            <button className="btn btn-secondary" onClick={logout}>
              Logout
            </button>
          </div>
        </header>

        <section className="card mt-6">
          <div className="flex gap-3">
            <input
              className="input flex-1"
              placeholder="New task..."
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              disabled={loading || mutating}
              onKeyDown={(e) => {
                if (e.key === "Enter") addTask();
              }}
            />
            <button
              className="btn btn-primary"
              onClick={addTask}
              disabled={loading || mutating}
            >
              {mutating ? "..." : "Add"}
            </button>
          </div>

          {error && <div className="alert-error mt-3">{error}</div>}
        </section>

        <section className="mt-6">
          {loading ? (
            <div className="muted">Loading...</div>
          ) : tasks.length === 0 ? (
            <div className="muted">alredy don't have new tasks</div>
          ) : (
            <ul className="space-y-3">
              {tasks.map((t) => (
                          <li key={t.id} className="card flex items-center justify-between">
            <div className="flex items-center gap-3">
              
              <input
                type="checkbox"
                checked={t.isCompleted}
                disabled={mutating}
                onChange={() => toggleTask(t)}
              />
              <div
                className={
                  "h-2.5 w-2.5 rounded-full " +
                  (t.isCompleted ? "bg-emerald-500" : "bg-slate-600")
                }
              />

              <div>
                <div className={t.isCompleted ? "text-slate-400 line-through" : "font-medium"}>
                  {t.title}
                </div>

                <div className="text-xs text-slate-500 mt-1">
                  {new Date(t.createdAtUtc).toLocaleString()}
                  {" • "}
                  <span className={t.isCompleted ? "text-emerald-300" : "text-slate-400"}>
                    {t.isCompleted ? "Done" : "Active"}
                  </span>
                </div>
              </div>
            </div>

            <div className="flex items-center gap-3">
              <div className="text-xs text-slate-500">#{t.id}</div>

              <button
                className="btn btn-secondary"
                disabled={mutating}
                onClick={() => removeTask(t.id)}
              >
                Delete
              </button>
            </div>
          </li>
                        ))}
            </ul>
          )}
        </section>
      </div>
    </div>
  );
}