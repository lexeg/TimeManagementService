import { useState } from "react";
import {
  createTask as createTaskApi,
  type CreateTaskRequest,
} from "../api/tasksApi";

interface TaskFormProps {
  onCreated: () => Promise<void>;
}

function TaskForm({ onCreated }: TaskFormProps) {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [tags, setTags] = useState("");
  const [deadlineAt, setDeadlineAt] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);

  const handleSubmit = async () => {
    if (!title.trim()) {
      setError("Title is required");
      return;
    }

    setError(null);
    setSaving(false);

    const task: CreateTaskRequest = {
      title: title.trim(),
      description: description.trim(),
      tags: tags.trim(),
      deadlineAt: deadlineAt || undefined,
    };

    try {
      await createTaskApi(task);

      setTitle("");
      setDescription("");
      setTags("");
      setDeadlineAt("");

      await onCreated();
    } catch (error) {
      console.error(error);
      setError("Failed to create task");
    } finally {
      setSaving(false);
    }
  };

  return (
    <section className="create-task">
      <div>
        <label htmlFor="title">Title</label>
        <input
          id="title"
          type="text"
          placeholder="Enter task title..."
          value={title}
          onChange={(event) => setTitle(event.target.value)}
          onKeyDown={(event) => {
            if (event.key === "Enter") {
              handleSubmit();
            }
          }}
        />
      </div>
      <div>
        <label htmlFor="description">Description</label>
        <textarea
          id="description"
          placeholder="Enter task description"
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

      <button type="button" onClick={handleSubmit} disabled={saving}>
        {saving ? "Adding..." : "Add task"}
      </button>

      {error && <div className="error">{error}</div>}
    </section>
  );
}

export default TaskForm;
