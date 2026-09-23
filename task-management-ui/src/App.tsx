import { useState } from "react";
import "./App.css";
import type { Task } from "./types/task";
import { useTasks } from "./hooks/useTasks";
import TaskList from "./components/TaskList";
import TaskForm from "./components/TaskForm";

function App() {
  const {
    tasks,
    loading,
    error,
    operationError,
    loadTasks,
    createTask,
    updateTask,
    deleteTask,
    updateTaskStatus,
  } = useTasks();
  const [editingTask, setEditingTask] = useState<Task | null>(null);

  const handleSaved = async () => {
    setEditingTask(null);
  };

  const handleEdit = async (task: Task) => {
    setEditingTask(task);
  };

  const handleCancelEdit = async () => {
    setEditingTask(null);
  };

  const handleDelete = async (id: number) => {
    const confirmed = window.confirm(
      "Are you sure you want to delete this task?",
    );
    if (!confirmed) {
      return;
    }
    await deleteTask(id);
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

      {operationError && (
        <div className="operation-error">{operationError}</div>
      )}

      <main>
        <TaskForm
          task={editingTask ?? undefined}
          onCreate={createTask}
          onUpdate={updateTask}
          onSaved={handleSaved}
          onCancel={handleCancelEdit}
        />

        <section>
          <h2>Tasks</h2>

          <TaskList
            tasks={tasks}
            onDelete={handleDelete}
            onEdit={handleEdit}
            onStatusChange={updateTaskStatus}
          />
        </section>
      </main>
    </div>
  );
}

export default App;
