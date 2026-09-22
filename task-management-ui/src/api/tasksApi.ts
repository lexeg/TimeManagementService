import type { Task } from "../types/task";

const API_URL = "/api/Tasks/tasks";

export interface CreateTaskRequest {
  title: string;
  description?: string;
  tags?: string;
  deadlineAt?: string;
}

export interface UpdateTaskRequest {
  title: string;
  description?: string;
  status: Task["status"];
  tags?: string;
  deadlineAt?: string;
}

export async function getTasks(): Promise<Task[]> {
  const response = await fetch(API_URL);

  if (!response.ok) {
    throw new Error("Failed to load tasks");
  }

  return response.json();
}

export async function createTask(task: CreateTaskRequest): Promise<void> {
  const response = await fetch(API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(task),
  });
  if (!response.ok) {
    throw new Error("Failed to create task");
  }
}

export async function updateTask(
  id: number,
  task: UpdateTaskRequest,
): Promise<void> {
  const response = await fetch(`${API_URL}/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(task),
  });

  if (!response.ok) {
    throw new Error("Failed to update task");
  }
}

export async function deleteTask(id: number): Promise<void> {
  const response = await fetch(`${API_URL}/${id}`, { method: "DELETE" });
  if (!response.ok) {
    throw new Error("Failed to delete task");
  }
}
