#!/bin/bash
REMOTE=origin
BRANCH=master
cd ./../home/ubuntu/RMUD2
git fetch
pid=$(sudo lsof -t -i:2001)
echo "Existing process: $pid"
if [[ "$(git rev-parse $BRANCH)" != "$(git rev-parse "$REMOTE/$BRANCH")" ]] || [ -z "$pid" ]; then
    rm -rf ./cd.log
    echo "Deploying new changes at $(date): $(git log -1 --pretty=%B)"
    bash start.sh &
fi

#eof