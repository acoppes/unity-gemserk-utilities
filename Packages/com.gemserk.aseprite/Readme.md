# Unity Aseprite Importer

It is basically a helper to run Aseprite export command line tools from within Unity.

## Aseprite Import Data

Configure a source folder where multiple .aseprite files are, an output folder inside Unity assets and a format to use following the asprite valid export format, for example `{title}_{tag}_{tagframe0001}.png`, and it will run Aseprite.exe to export a list of png following that format.