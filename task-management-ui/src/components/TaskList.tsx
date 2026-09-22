import type { Task } from "../types/task";
import TaskItem from "./TaskItem";

interface TaskListProps {
  tasks: Task[];
  onDelete: (id: number) => void;
  onEdit: (task: Task) => void;
  onStatusChange: (id: number, status: Task["status"]) => Promise<void>;
}

function TaskList({ tasks, onDelete, onEdit, onStatusChange }: TaskListProps) {
  if (tasks.length === 0) {
    return <div className="empty">No tasks yet</div>;
  }

  return (
    <div className="task-list">
      {tasks.map((task) => (
        <TaskItem
          key={task.id}
          task={task}
          onDelete={onDelete}
          onEdit={onEdit}
          onStatusChange={onStatusChange}
        />
      ))}
    </div>
  );
}

export default TaskList;
