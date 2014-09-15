The project structure reorganization has a few purposes:

- decoupling Windows Forms implementation from the entire application,
  so that we can switch to other GUI frameworks. Switching to WPF is
  the first step to go, then we can try to port it to Xwt later.
- decoupling Windows-specific implementation, so that we can adopt this
  to non-Windows vocal synthesizers, if they happened.

Currently Cadencii.Windows.Forms and Cadencii.Application.Model are
winforms-only implementation. Everything else is detached from winforms.

For MIDI operations, we could use managed-midi cross-platform MIDI toolkit
(once portmidi or rtmidi-c could be shipped for every platform).
Also for audio operations, we could similarly use portaudio-sharp
(once portaudio could be shipped for every platform).
Though use of existing Windows-specific API (Cadencii.Media.Windows and
Cadencii.Platform.Windows) is tightly coupled with the rest of the code,
so I would deal with them later.
There wouldn't be non-Windows synthesizer anytime soon anyways.

