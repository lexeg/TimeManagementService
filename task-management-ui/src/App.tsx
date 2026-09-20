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

  const loadTasks = async () => {
    try {
      const data = await getTasks();
      setTasks(data);
    } catch (error) {
      console.error(error);
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

    try {
      await createTaskApi({
        title: title.trim(),
        description: "",
      });

      setTitle("");
      await loadTasks();
    } catch (error) {
      console.error(error);
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
