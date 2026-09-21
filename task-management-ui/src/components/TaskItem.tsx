import type { Task } from "../types/task";
import TaskStatus from "./TaskStatus";
import { formatLocalDateTime } from "../utils/dateUtils";

interface TaskItemProps {
  task: Task;
  onDelete: (id: number) => void;
}

function TaskItem({ task, onDelete }: TaskItemProps) {
  return (
    <div className="task" key={task.id}>
      <div>
        <div className="task-title">{task.title}</div>
        <TaskStatus status={task.status} />
        {task.description && (
          <div className="task-description">{task.description}</div>
        )}
      </div>

      <div className="task-meta">
        <div>Created: {formatLocalDateTime(task.createdAt)}</div>

        {task.deadlineAt && (
          <div>Deadline: {formatLocalDateTime(task.deadlineAt)}</div>
        )}
      </div>

      <button className="delete-button" onClick={() => onDelete(task.id)}>
        Delete
      </button>
    </div>
  );
}

export default TaskItem;
