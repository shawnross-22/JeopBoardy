# JeopBoardy: App for playing real *Jeopardy!* games #

Run using ``obj/Debug/JeopBoardy.exe``. Game IDs here match the ID from the URL in the *J! Archive* &mdash; note that these are different than the serial game number assigned in the archive. Source code is largely stored in ``MainWindow.xaml.cs``.

``data/`` contains the clue data that feeds the app in ``jData.txt`` and functions that can be used to pull it &mdash; ``gameIDs.R`` pulls game IDs by season, and ``getJData.R`` takes a vector of game IDs and returns the data with one line per clue.