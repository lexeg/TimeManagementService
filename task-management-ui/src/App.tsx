import { useEffect, useState } from "react";
import "./App.css";

interface Task {
  id: number;
  title: string;
  description?: string;
}

const API_URL = "http://localhost:5111/api/Tasks";

function App() {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [title, setTitle] = useState("");
  const [loading, setLoading] = useState(true);

  const loadTasks = async () => {
    try {
      const response = await fetch(`${API_URL}/tasks`);

      if (!response.ok) {
        throw new Error("Failed to load tasks");
      }

      const data = await response.json();
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

    const task: Task = {
      id: 0,
      title: title.trim(),
    };

    const response = await fetch(`${API_URL}/tasks`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(task),
    });

    if (!response.ok) {
      console.error("Failed to create task");
      return;
    }

    setTitle("");
    await loadTasks();
  };

  const deleteTask = async (id: number) => {
    const response = await fetch(`${API_URL}/tasks/${id}`, {
      method: "DELETE",
    });

    if (!response.ok) {
      console.error("Failed to delete task");
      return;
    }

    setTasks(tasks.filter((x) => x.id !== id));
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

          {tasks.length === 0 ? (
            <div className="empty">No tasks yet</div>
          ) : (
            <div className="task-list">
              {tasks.map((task) => (
                <div className="task" key={task.id}>
                  <div>
                    <div className="task-title">{task.title}</div>

                    {task.description && (
                      <div className="task-description">{task.description}</div>
                    )}
                  </div>

                  <button
                    className="delete-button"
                    onClick={() => deleteTask(task.id)}
                  >
                    Delete
                  </button>
                </div>
              ))}
            </div>
          )}
        </section>
      </main>
    </div>
  );
}

export default App;
