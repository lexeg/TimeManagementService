import { useEffect, useState } from "react";
import "./App.css";
import type { Task } from "./types/task";
import {
  getTasks,
  createTask as createTaskApi,
  deleteTask as deleteTaskApi,
} from "./api/tasksApi";
import TaskList from "./components/TaskList";

function App() {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [title, setTitle] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [createError, setCreateError] = useState<string | null>(null);

  const loadTasks = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await getTasks();
      setTasks(data);
    } catch (error) {
      console.error(error);
      setError("Failed to load tasks");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadTasks();
  }, []);

  const createTask = async () => {
    if (!title.trim()) {
      return;
    }

    setCreateError(null);

    try {
      await createTaskApi({
        title: title.trim(),
        description: "",
      });

      setTitle("");
      await loadTasks();
    } catch (error) {
      console.error(error);
      setCreateError("Failed to create tasks");
    }
  };

  const deleteTask = async (id: number) => {
    try {
      await deleteTaskApi(id);

      setTasks((currentTasks) => currentTasks.filter((task) => task.id !== id));
    } catch (error) {
      console.error(error);
    }
  };

  if (loading) {
    return <div className="loading">Loading...</div>;
  }

  if (error) {
    return (
      <div className="error">
        <p>{error}</p>
        <button onClick={loadTasks}>Retry</button>
      </div>
    );
  }

  return (
    <div className="app">
      <header>
        <h1>Task Management</h1>
        <p>Manage your tasks</p>
      </header>

      <main>
        <section className="create-task">
          <input
            type="text"
            placeholder="Enter task title..."
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                createTask();
              }
            }}
          />

          <button onClick={createTask}>Add</button>

          {createError && <div className="error">{createError}</div>}
        </section>

        <section>
          <h2>Tasks</h2>

          <TaskList tasks={tasks} onDelete={deleteTask} />
        </section>
      </main>
    </div>
  );
}

export default App;
