#### Every 0.5s it
- Queries Livesplit Server with 'getcurrenttimerphase' to check Livesplit is running
- Gets the current split's index with 'getsplitindex' 
- Finds the newest recording in the recordings folder
- Tests if the previous index is not equal to the current index (returns true the first time)
- If true, set the previous index to current index (else set last time to the last split's time)
- Get the split name with 'getprevioussplitname'
- Get the current time with 'getcurrenttime'
- Set the total time to current time - last time + 2
- Test the time in the matching filename that the current time is lower
- If true, make a backup of the old file, and begin the cutting process

#### In the cutting process it
- Gets the current exact datetime
- Copies the recording to a temporary file
- Cuts from the end by starting ffmpeg at the duration of temp recording - total time of the split
- Gets the original temp file's duration
- Sets the start time for cutting in the future to whatever the start time is currently + duration of the temp file - 2 (important for later when cutting again later from the same recording)

#### Some known bugs are
- Won't work with the very first and very last splits
- Cutting is usually inaccurate, meaning manual trimming is required
- Desyncs after a couple hours of recording
- Won't work well with extremely short (around 5s or less) splits

Currently uses index_timeinseconds.ms_splitname for filename convention. For example '13_53.25_Heavy Metal Cave.mkv'. A better option would be to use a CSV file containing filenames and times.
