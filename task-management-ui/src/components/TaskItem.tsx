import type { Task } from "../types/task";

interface TaskItemProps {
  task: Task;
  onDelete: (id: number) => void;
}

function TaskItem({ task, onDelete }: TaskItemProps) {
  return (
    <div className="task" key={task.id}>
      <div>
        <div className="task-title">{task.title}</div>

        {task.description && (
          <div className="task-description">{task.description}</div>
        )}
      </div>

      <button className="delete-button" onClick={() => onDelete(task.id)}>
        Delete
      </button>
    </div>
  );
}

export default TaskItem;
