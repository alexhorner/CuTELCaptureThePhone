#!/bin/bash
rm -rf ./sounds
find ./rawsounds -type f -name "*.mp3" -exec bash -c 'f="{}"; out="./sounds/${f#./rawsounds/}"; mkdir -p "$(dirname "$out")"; lame --decode "$f" "${out%.*}.wav" && sox "${out%.*}.wav" "${out%.*}.wav" gain 5 rate 8000' \;
