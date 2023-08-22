#!/bin/bash
clear
echo "Clean"
RENAME_OLD="NWDWebUploader"
RENAME_NEW="BBOWebUploader"

RENAME_OLD_B="NWDWebDownloader"
RENAME_NEW_B="BBOWebDownloader"

# Memorize this script path 
SCRIPT_DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )
SOURCE_DIR="${SCRIPT_DIR}/.."
# go to Root of dir
cd "${SOURCE_DIR}"
echo "SOURCE_DIR : ${SOURCE_DIR}"
# clean .DS_Store
find . -name '.DS_Store' -type f -delete
find . -name 'obj' -type d -delete
find . -name 'bin' -type d -delete
for dir in `find . -name 'obj' -type d`
do
    #echo $dir
    rm -rf $dir
done
for dir in `find . -name 'bin' -type d`
do
    #echo $dir
    rm -rf $dir
done

find . -type f -name '*.*' | sed 's|.*\.||' | sort -u
echo "Copy all in new project ... "
THIS_DIR=$(pwd)
THIS_DIR_COPY="${THIS_DIR}_Renamed"
echo "THIS_DIR ${THIS_DIR}"
echo "THIS_DIR_COPY ${THIS_DIR_COPY}"
mkdir ${THIS_DIR_COPY}
cp -r ${THIS_DIR}/* ${THIS_DIR_COPY}

cd ${THIS_DIR_COPY}

DIR_ROOT="."
for i in 1 2 3 4 5 6 7 8 9 10 11
do
  DIR_ROOT="${DIR_ROOT}/*"
  echo "Analyze directories in  ${DIR_ROOT}"
  for DIRECTORY in ${DIR_ROOT}; do
    if [ -d "${DIRECTORY}" ] ; then
        NEW_DIRECTORY=${DIRECTORY//$RENAME_OLD/$RENAME_NEW}
        echo "${DIRECTORY} is a directory and must be rename to ${NEW_DIRECTORY}"
        mv -f ${DIRECTORY} ${NEW_DIRECTORY}
    fi
  done
done

DIR_ROOT="."
for i in 1 2 3 4 5 6 7 8 9 10 11
do
  DIR_ROOT="${DIR_ROOT}/*"
  echo "Analyze directories in  ${DIR_ROOT}"
  for DIRECTORY in ${DIR_ROOT}; do
    if [ -d "${DIRECTORY}" ] ; then
        NEW_DIRECTORY=${DIRECTORY//$RENAME_OLD_B/$RENAME_NEW_B}
        echo "${DIRECTORY} is a directory and must be rename to ${NEW_DIRECTORY}"
        mv -f ${DIRECTORY} ${NEW_DIRECTORY}
    fi
  done
done


DIR_ROOT="."
for j in 1 2 3 4 5 6 7 8 9 10 11
do
  DIR_ROOT="${DIR_ROOT}/*"
  echo "Analyze files in ${DIR_ROOT}"
  for FILENAME in ${DIR_ROOT}; do
    if [ -f "${FILENAME}" ]; then
        NEW_FILENAME=${FILENAME//$RENAME_OLD/$RENAME_NEW}
        echo "${FILENAME} is a file and must be rename to ${NEW_FILENAME}"
        PATTERN_EXTENSION="^.*/.*\.(Config|cs|cshtml|csproj|git|gitignore|html|json|md|scss|sh|sln|rtf|txt|xhtml|xml|yml)$"
        if [[ ${FILENAME} =~ $PATTERN_EXTENSION ]];
        then 
          echo "${FILENAME} is a file and must be analyze to replace ${RENAME_OLD} by ${RENAME_NEW}"
          sed -i .bak "s|${RENAME_OLD}|${RENAME_NEW}|g" "${FILENAME}"
          rm -f ${FILENAME}.bak
        fi
        mv -f ${FILENAME} ${NEW_FILENAME}
    fi
  done
done

DIR_ROOT="."
for j in 1 2 3 4 5 6 7 8 9 10 11
do
  DIR_ROOT="${DIR_ROOT}/*"
  echo "Analyze files in ${DIR_ROOT}"
  for FILENAME in ${DIR_ROOT}; do
    if [ -f "${FILENAME}" ]; then
        NEW_FILENAME=${FILENAME//$RENAME_OLD_B/$RENAME_NEW_B}
        echo "${FILENAME} is a file and must be rename to ${NEW_FILENAME}"
        PATTERN_EXTENSION="^.*/.*\.(Config|cs|cshtml|csproj|git|gitignore|html|json|md|scss|sh|sln|rtf|txt|xhtml|xml|yml)$"
        if [[ ${FILENAME} =~ $PATTERN_EXTENSION ]];
        then 
          echo "${FILENAME} is a file and must be analyze to replace ${RENAME_OLD_B} by ${RENAME_NEW_B}"
          sed -i .bak "s|${RENAME_OLD_B}|${RENAME_NEW_B}|g" "${FILENAME}"
          rm -f ${FILENAME}.bak
        fi
        mv -f ${FILENAME} ${NEW_FILENAME}
    fi
  done
done

open .

echo "Finish! Enjoy!"