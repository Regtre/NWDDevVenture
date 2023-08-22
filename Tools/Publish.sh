#!/bin/bash

#GIT_STRATEGY="clone"
GIT_STRATEGY="fetch"

echo "==================================================================="
echo "                            Let's go!"
echo "==================================================================="

SCRIPT_DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )
cd ${SCRIPT_DIR}
UpdateDate=$(date '+%Y-%m-%d %H:%M:%S')
sed -i.bak "s|<DateTime>.*</DateTime>|<DateTime>${UpdateDate}</DateTime>|g" ../.gitlab-ci.yml
sed -i.bak "s|GIT_STRATEGY\:.*|GIT_STRATEGY: ${GIT_STRATEGY}|g" ../.gitlab-ci.yml
rm ../.gitlab-ci.yml.bak
git status
git add -u
git commit -m "[PUBLISH] Publish.sh commit command"
git push origin main

echo "==================================================================="
echo "Finish! Enjoy!"
echo "==================================================================="

