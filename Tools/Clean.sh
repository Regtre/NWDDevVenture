#!/bin/bash
clear
echo "Clean"

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
    echo $dir
    rm -rf $dir
done
for dir in `find . -name 'bin' -type d`
do
    echo $dir
    rm -rf $dir
done

dotnet nuget locals all -l
dotnet nuget locals all --clear

open .
echo "Finish! Enjoy!"