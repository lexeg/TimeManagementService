#!/bin/bash

set -e

echo "========================================"
echo "Starting deployment"
echo "========================================"

cd /var/www/tms

echo "----------------------------------------"
echo "Updating repository"
echo "----------------------------------------"

git fetch origin
git reset --hard origin/master

echo "Current commit:"
git rev-parse --short HEAD

echo "----------------------------------------"
echo "Starting PostgreSQL"
echo "----------------------------------------"

docker compose up -d postgres

echo "----------------------------------------"
echo "Creating database"
echo "----------------------------------------"

docker compose run --rm postgres-create-db

echo "Database initialization completed."

echo "----------------------------------------"
echo "Applying database migrations"
echo "----------------------------------------"

docker compose run --rm postgres-migrations

echo "Database migrations applied successfully."

echo "----------------------------------------"
echo "Building application containers"
echo "----------------------------------------"

docker compose build timemanagementservice frontend

echo "----------------------------------------"
echo "Starting application"
echo "----------------------------------------"

docker compose up -d timemanagementservice frontend

echo "----------------------------------------"
echo "Removing unused Docker images"
echo "----------------------------------------"

docker image prune -f

echo "========================================"
echo "Deployment completed successfully"
echo "========================================"

echo "Running containers:"
docker compose ps