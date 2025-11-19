#!/bin/bash

#
#   INFO:
#       This scrip will be executed *inside* the localstack container once all its services are up and running
#

set -e

export AWS_DEFAULT_REGION=us-east-1
export AWS_ACCESS_KEY_ID=test
export AWS_SECRET_ACCESS_KEY=test

endpoint="--endpoint-url=http://localhost:4566"

echo "Initializing LocalStack secrets..."

aws $endpoint secretsmanager create-secret --name MartenDbPassword \
  --secret-string 'mypwd'
echo "MartenDbPassword secret created!"

echo "LocalStack initialization complete!"