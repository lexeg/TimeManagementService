export enum TaskStatus {
  New = 0,
  InProgress = 1,
  Completed = 2,
}

export interface Task {
  id: number;
  title: string;
  description?: string;
  tags?: string;
  status: TaskStatus;
  createdAt: Date;
  deadlineAt?: Date;
}
