#!/bin/bash

set -e

export AWS_ACCESS_KEY_ID=test
export AWS_SECRET_ACCESS_KEY=test

endpoint="--endpoint-url=http://localhost:4566"

echo "Initializing LocalStack secrets..."

aws $endpoint secretsmanager create-secret \
  --name MartenDbPassword \
  --secret-string "mypwd" \
  || echo "MartenDbPassword already exists"

aws $endpoint secretsmanager create-secret \
  --name EmailSmtpPassword \
  --secret-string "" \
  || echo "EmailSmtpPassword already exists"

echo "LocalStack initialization complete!"