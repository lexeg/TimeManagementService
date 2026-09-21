import { TaskStatus as TaskStatusType } from "../types/task";

interface TaskStatusProps {
  status: TaskStatusType;
}

function TaskStatus({ status }: TaskStatusProps) {
  switch (status) {
    case TaskStatusType.New:
      return <span className="task-status">New</span>;
    case TaskStatusType.InProgress:
      return <span className="task-status">InProgress</span>;
    case TaskStatusType.Completed:
      return <span className="task-status">Completed</span>;
    default:
      return <span className="task-status">Unknown</span>;
  }
}

export default TaskStatus;
