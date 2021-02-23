#!/bin/bash

# Create migrations directory
MIGRATION_PATH=/db/migrations
if [ -d $MIGRATION_PATH ]; then
    rm -rf $MIGRATION_PATH
fi
mkdir -p $MIGRATION_PATH

# Integrate model with file "integrate.rc"
FILENAME='integrate.rc'
if [ ! -f $FILENAME ]; then
    echo "copy by example file"
    cp $FILENAME.example $FILENAME
fi
while IFS=$'\n' read -r line || [[ -n "$line" ]]; do
    if [ -d "./$line" ]; then
        echo "> copy $line to migrateions"
        if [ -d "./$line/scheme" ]; then
            echo ">> copy scheme"
            cp ./$line/scheme/* $MIGRATION_PATH
        fi
        if [ -d "./$line/test" ]; then
            echo ">> copy test"
            cp ./$line/test/* $MIGRATION_PATH
        fi
    fi
done < $FILENAME
