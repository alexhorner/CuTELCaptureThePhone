#!/bin/bash
rm -rf ./sounds
find ./rawsounds -type f -name "*.mp3" -exec bash -c 'f="{}"; out="./sounds/${f#./rawsounds/}"; tmp_out="${out%.*}_tmp.wav"; mkdir -p "$(dirname "$out")" && lame --decode "$f" "${out%.*}.wav" && sox "${out%.*}.wav" "$tmp_out" gain 5 rate 8000 && mv "$tmp_out" "${out%.*}.wav" || echo "Error processing $f"' \;
