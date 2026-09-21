import { useEffect, useState } from "react";
import "./App.css";
import type { Task } from "./types/task";
import { getTasks, deleteTask as deleteTaskApi } from "./api/tasksApi";
import TaskList from "./components/TaskList";
import TaskForm from "./components/TaskForm";

function App() {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

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
        <TaskForm onCreated={loadTasks} />

        <section>
          <h2>Tasks</h2>

          <TaskList tasks={tasks} onDelete={deleteTask} />
        </section>
      </main>
    </div>
  );
}

export default App;
