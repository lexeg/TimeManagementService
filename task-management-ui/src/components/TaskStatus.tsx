import { TaskStatus as TaskStatusType } from "../types/task";

interface TaskStatusProps {
  status: TaskStatusType;
  onChange?: (status: TaskStatusType) => void;
}

function TaskStatus({ status, onChange }: TaskStatusProps) {
  const getStatusText = () => {
    switch (status) {
      case TaskStatusType.New:
        return "New";
      case TaskStatusType.InProgress:
        return "InProgress";
      case TaskStatusType.Completed:
        return "Completed";
      default:
        return "Unknown";
    }
  };

  if (!onChange) {
    return <span className="task-status">{getStatusText()}</span>;
  }

  return (
    <select
      className="task-status"
      value={status}
      onChange={(event) =>
        onChange(Number(event.target.value) as TaskStatusType)
      }
    >
      <option value={TaskStatusType.New}>New</option>
      <option value={TaskStatusType.InProgress}>InProgress</option>
      <option value={TaskStatusType.Completed}>Completed</option>
    </select>
  );
}

export default TaskStatus;
