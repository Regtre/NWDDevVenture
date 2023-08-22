#!/bin/bash

DIRPROJECT="/Users/jfcidemobi/Desktop/NWDVenture/NWDDevVenture/NWDWeb/"
DIR="/Users/jfcidemobi/Desktop/NWDVenture/Design/"

#FALCON=falcon-v3.14.0
FALCON=falcon-v3.17.0
DIRWWWROOT="${DIRPROJECT}/wwwroot/"
FILE="${DIRPROJECT}/_user-variables.scss"

if test -f "$FILE";
then
    echo "$FILE user variables exists."
else
    touch $FILE
    echo -e "// Any variable from node_modules/bootstrap/scss/variables or src/scss/theme/_variables.scss can be overridden with your own value." >> $FILE
    echo "$FILE user variables just create."
fi

echo "Create temporary directory"
TEMP="${DIRPROJECT}/TmpFalconInstall/"
sudo rm -rf $TEMP
mkdir -p $TEMP

echo "Copy Falcon inside"
cp -r "${DIR}${FALCON}" $TEMP

echo "Copy user variables file"
cp -r $FILE $TEMP/$FALCON/src/scss/
cd $TEMP/$FALCON
# open .


which -s brew
if [[ $? != 0 ]] ; then
    echo "Install Brew"
    /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
else
    brew update
fi

which -s node
if [[ $? != 0 ]] ; then
    echo "Install Node"
    brew update
    brew install node
    node -v
    npm -v
fi

which -s gulp
if [[ $? != 0 ]] ; then
    echo "Install Gulp"
    sudo npm install gulp -g
    sudo npm install gulp-cli -g
    sudo npm install gulp --save-dev
fi

which -s convert
if [[ $? != 0 ]] ; then
    echo "Install Convert"
brew install convert
fi

which -s magick
if [[ $? != 0 ]] ; then
    echo "Install Magick"
brew install magick
fi

echo "Icon generation"
magick "${DIRWWWROOT}/favicons/icon80@3x.png" -adaptive-resize 192x192 "${DIRWWWROOT}/favicons/android-chrome-192x192.png"
magick "${DIRWWWROOT}/favicons/icon80.svg" -adaptive-resize 512x512 "${DIRWWWROOT}/favicons/android-chrome-512x512.png"
magick "${DIRWWWROOT}/favicons/icon80@3x.png" -adaptive-resize 180x180 "${DIRWWWROOT}/favicons/apple-touch-icon.png"
magick "${DIRWWWROOT}/favicons/icon80@3x.png" -adaptive-resize 16x16 "${DIRWWWROOT}/favicons/favicon-16x16.png"
magick "${DIRWWWROOT}/favicons/icon80@3x.png" -adaptive-resize 32x32 "${DIRWWWROOT}/favicons/favicon-32x32.png"
magick "${DIRWWWROOT}/favicons/icon80@3x.png" -adaptive-resize 48x48 "${DIRWWWROOT}/favicons/favicon-48x48.png"
magick "${DIRWWWROOT}/favicons/icon80@3x.png" -adaptive-resize 80x80 "${DIRWWWROOT}/favicons/favicon-80x80.png"
magick "${DIRWWWROOT}/favicons/icon80@3x.png" -adaptive-resize 150x150 "${DIRWWWROOT}/favicons/mstile-150x150.png"
convert -resize x16 -gravity center -crop 16x16+0+0 "${DIRWWWROOT}/favicons/icon80@3x.png" -flatten -colors 256 -background transparent $DIRWWWROOT/favicon.ico

echo "Gulp generation"
# npm install -g gulp
sudo npm link gulp
sudo gulp  clean
sudo gulp  compile
#gulp  style
#gulp  script
#gulp  script:webpack
#gulp  vendor

if [ -d "${DIRWWWROOT}" ];
then
    echo "${DIRWWWROOT} directory exists."
    rm -r ${DIRWWWROOT}/assets
    cp -R ${TEMP}/${FALCON}/public/assets ${DIRWWWROOT}
    rm -r ${DIRWWWROOT}/assets/data
    rm -r ${DIRWWWROOT}/assets/logos
    rm -r ${DIRWWWROOT}/assets/video
    rm -r ${DIRWWWROOT}/vendors
    cp -R ${TEMP}/${FALCON}/public/vendors ${DIRWWWROOT}
else
    echo "${DIRWWWROOT} directory does not exist."
    exit
fi

echo "Flush temporary directory"
sudo rm -rf $TEMP

#open ${DIRPROJECT}
echo "Finish! Enjoy!"
