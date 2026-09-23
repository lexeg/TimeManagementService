import { useEffect, useState } from "react";
import {
  createTask as createTaskApi,
  deleteTask as deleteTaskApi,
  getTasks,
  updateTask as updateTaskApi,
  type CreateTaskRequest,
  type UpdateTaskRequest,
} from "../api/tasksApi";
import type { Task } from "../types/task";

interface UseTasksResult {
  tasks: Task[];
  loading: boolean;
  error: string | null;
  operationError: string | null;
  loadTasks: () => Promise<void>;
  createTask: (task: CreateTaskRequest) => Promise<void>;
  updateTask: (id: number, task: UpdateTaskRequest) => Promise<void>;
  deleteTask: (id: number) => Promise<void>;
  updateTaskStatus: (id: number, status: Task["status"]) => Promise<void>;
}

export function useTasks(): UseTasksResult {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [operationError, setOperationError] = useState<string | null>(null);

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

  const createTask = async (task: CreateTaskRequest) => {
    try {
      setOperationError(null);
      await createTaskApi(task);
      await loadTasks();
    } catch (error) {
      console.error(error);
      setOperationError("Failed to create task");
      throw error;
    }
  };

  const updateTask = async (id: number, task: UpdateTaskRequest) => {
    try {
      setOperationError(null);
      await updateTaskApi(id, task);
      await loadTasks();
    } catch (error) {
      console.error(error);
      setOperationError("Failed to update task");
      throw error;
    }
  };

  const deleteTask = async (id: number) => {
    try {
      setOperationError(null);
      await deleteTaskApi(id);

      setTasks((currentTasks) => currentTasks.filter((task) => task.id !== id));
    } catch (error) {
      console.error(error);
      setOperationError("Failed to delete task");
      throw error;
    }
  };

  const updateTaskStatus = async (id: number, status: Task["status"]) => {
    setOperationError(null);
    const task = tasks.find((task) => task.id === id);

    if (!task) {
      return;
    }

    try {
      await updateTaskApi(id, {
        title: task.title,
        description: task.description,
        tags: task.tags,
        status,
        deadlineAt: task.deadlineAt,
      });

      setTasks((currentTasks) =>
        currentTasks.map((currentTask) =>
          currentTask.id === id ? { ...currentTask, status } : currentTask,
        ),
      );
    } catch (error) {
      console.error(error);
      setOperationError("Failed to update task");
      throw error;
    }
  };

  useEffect(() => {
    loadTasks();
  }, []);

  return {
    tasks,
    loading,
    error,
    operationError,
    loadTasks,
    createTask,
    updateTask,
    deleteTask,
    updateTaskStatus,
  };
}
