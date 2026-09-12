SELECT 'CREATE DATABASE "TasksDB"'
WHERE NOT EXISTS (
    SELECT FROM pg_database WHERE datname = 'TasksDB'
)\gexec