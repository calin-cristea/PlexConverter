# PlexConverter
PlexConverter provides a straightforward command line tool for handling media for your Plex Media Server, making tasks such as converting to a supported media container, with proper video and audio codecs.
## Features
* Supports any media format as input
* Outputs to MP4 (default) or matroska (`-mkv` switch)
## Requirements
* Windows, MacOS or Linux
* `ffmpeg` (and `ffprobe`) [installed](https://ffmpeg.org/download.html) and available in `PATH`
* `nvencc` [installed](https://github.com/rigaya/NVEnc/releases) and available in `PATH`
* `mp4box` [installed](https://gpac.wp.imt.fr/downloads/) and available in `PATH` (for MP4 output)
## Usage
`PlexConverter "/path/to/media.ext" [-mkv]`
