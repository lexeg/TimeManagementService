import type { Task } from "../types/task";
import TaskStatus from "./TaskStatus";
import { formatLocalDateTime } from "../utils/dateUtils";

interface TaskItemProps {
  task: Task;
  onDelete: (id: number) => void;
  onEdit: (task: Task) => void;
  onStatusChange: (id: number, status: Task["status"]) => Promise<void>;
}

function TaskItem({ task, onDelete, onEdit, onStatusChange }: TaskItemProps) {
  return (
    <div className="task">
      <div>
        <div className="task-title">{task.title}</div>

        <TaskStatus
          status={task.status}
          onChange={(status) => onStatusChange(task.id, status)}
        />

        {task.description && (
          <div className="task-description">{task.description}</div>
        )}

        <div className="task-meta">
          <div>Created: {formatLocalDateTime(task.createdAt)}</div>

          {task.deadlineAt && (
            <div>Deadline: {formatLocalDateTime(task.deadlineAt)}</div>
          )}
        </div>
      </div>

      <div>
        <button className="edit-button" onClick={() => onEdit(task)}>
          Edit
        </button>

        <button className="delete-button" onClick={() => onDelete(task.id)}>
          Delete
        </button>
      </div>
    </div>
  );
}

export default TaskItem;
