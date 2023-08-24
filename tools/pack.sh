#!/bin/bash

# This script builds and packs the artifacts. Use when you have MSBuild installed.
version=$(cat version)
releaseconf=$1
if [ -z $releaseconf ]; then
	releaseconf=Release
fi

# Check for dependencies
rarpath=`which rar`
if [ ! $? == 0 ]; then
	echo rar is not found.
	exit 1
fi

# Pack binary
echo Packing binary...
"$rarpath" a -ep1 -r -m5 /tmp/$version-bin.rar "../SpaceMan/bin/$releaseconf/netstandard2.0/"
if [ ! $? == 0 ]; then
	echo Packing using rar failed.
	exit 1
fi

# Inform success
mv ~/tmp/$version-bin.rar .
echo Build and pack successful.
exit 0
