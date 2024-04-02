#!/bin/bash
REMOTE=origin
BRANCH=master
cd ./../home/ubuntu/RMUD2
git fetch
pid=$(sudo lsof -t -i:443)
if [[ "$(git rev-parse $BRANCH)" != "$(git rev-parse "$REMOTE/$BRANCH")" ]] || [["$pid" == ""]]; then
    rm -rf ./cd.log
    echo "Deploying new changes at $(date): $(git log -1 --pretty=%B)"
    bash start.sh &
fi