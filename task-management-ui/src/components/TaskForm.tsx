import { useEffect, useState } from "react";
import type { CreateTaskRequest, UpdateTaskRequest } from "../api/tasksApi";
import type { Task } from "../types/task";

interface TaskFormProps {
  task?: Task;
  onCreate: (task: CreateTaskRequest) => Promise<void>;
  onUpdate: (id: number, task: UpdateTaskRequest) => Promise<void>;
  onSaved: () => void;
  onCancel?: () => void;
}

function convertLocalDateTimeToUtc(dateTimeLocal: string): string | undefined {
  if (!dateTimeLocal) {
    return undefined;
  }

  return new Date(dateTimeLocal).toISOString();
}

function convertUtcToLocalDateTime(dateTimeUtc?: string): string {
  if (!dateTimeUtc) {
    return "";
  }

  const date = new Date(dateTimeUtc);

  const offset = date.getTimezoneOffset();

  const localDate = new Date(date.getTime() - offset * 60 * 1000);

  return localDate.toISOString().slice(0, 16);
}

function TaskForm({
  task,
  onCreate,
  onUpdate,
  onSaved,
  onCancel,
}: TaskFormProps) {
  const isEditMode = task !== undefined;

  const [title, setTitle] = useState(task?.title ?? "");

  const [description, setDescription] = useState(task?.description ?? "");

  const [tags, setTags] = useState(task?.tags ?? "");

  const [deadlineAt, setDeadlineAt] = useState(
    convertUtcToLocalDateTime(task?.deadlineAt),
  );

  const [error, setError] = useState<string | null>(null);

  const [saving, setSaving] = useState(false);

  useEffect(() => {
    setTitle(task?.title ?? "");
    setDescription(task?.description ?? "");
    setTags(task?.tags ?? "");

    setDeadlineAt(convertUtcToLocalDateTime(task?.deadlineAt));

    setError(null);
  }, [task]);

  const handleSubmit = async () => {
    if (!title.trim()) {
      setError("Title is required");
      return;
    }

    setError(null);
    setSaving(true);

    try {
      if (isEditMode && task) {
        const updateRequest: UpdateTaskRequest = {
          title: title.trim(),
          description: description.trim(),
          tags: tags.trim(),
          status: task.status,
          deadlineAt: convertLocalDateTimeToUtc(deadlineAt),
        };

        await onUpdate(task.id, updateRequest);
      } else {
        const createRequest: CreateTaskRequest = {
          title: title.trim(),
          description: description.trim(),
          tags: tags.trim(),
          deadlineAt: convertLocalDateTimeToUtc(deadlineAt),
        };

        await onCreate(createRequest);
      }

      onSaved();

      if (!isEditMode) {
        setTitle("");
        setDescription("");
        setTags("");
        setDeadlineAt("");
      }
    } catch (error) {
      console.error(error);

      setError(isEditMode ? "Failed to update task" : "Failed to create task");
    } finally {
      setSaving(false);
    }
  };

  return (
    <section className="create-task">
      <h2>{isEditMode ? "Edit task" : "Create task"}</h2>

      <div>
        <label htmlFor="title">Title</label>

        <input
          id="title"
          type="text"
          placeholder="Enter task title..."
          value={title}
          onChange={(event) => setTitle(event.target.value)}
        />
      </div>

      <div>
        <label htmlFor="description">Description</label>

        <textarea
          id="description"
          placeholder="Enter task description..."
          value={description}
          onChange={(event) => setDescription(event.target.value)}
        />
      </div>

      <div>
        <label htmlFor="tags">Tags</label>

        <input
          id="tags"
          type="text"
          placeholder="react, frontend, learning"
          value={tags}
          onChange={(event) => setTags(event.target.value)}
        />
      </div>

      <div>
        <label htmlFor="deadlineAt">Deadline</label>

        <input
          id="deadlineAt"
          type="datetime-local"
          value={deadlineAt}
          onChange={(event) => setDeadlineAt(event.target.value)}
        />
      </div>

      <div>
        <button type="button" onClick={handleSubmit} disabled={saving}>
          {saving ? "Saving..." : isEditMode ? "Save" : "Add task"}
        </button>

        {isEditMode && onCancel && (
          <button type="button" onClick={onCancel} disabled={saving}>
            Cancel
          </button>
        )}
      </div>

      {error && <div className="error">{error}</div>}
    </section>
  );
}

export default TaskForm;
