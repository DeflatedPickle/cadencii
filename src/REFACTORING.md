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

Projects / Assemblies
---------------------

- Cadencii.Utilities
	most common utility API
- Cadencii.Gui.Framework <- Cadencii.Utilities
	awt-based API
- Cadencii.Core <- Cadencii.Gui.Framework
	fundamental API for every other library
- Cadencii.Gui.XmlSerialization <- Cadencii.Gui.Framework, Cadencii.Core
	XML serialization support for the GUI framework.
- Cadencii.Gui.Framework.WindowsForms <- Cadencii.Gui.Framework
	GUI framework implementation based on winforms.
- Cadencii.Gui.Framework.WindowsForms.Shared
	a hacky shared API to provide conversion from awt Color to GDI+ Color.
- Cadencii.Platform.Windows
	provides a set of P/Invokes and marshals for Windows API.

- Cadencii.Media.Midi
	javax.sound.midi based API
- Cadencii.Media.Vsq <- Cadencii.Gui.Framework, Cadencii.Gui.XmlSerialization
	VSQ support library.
- Cadencii.Media.Windows <- Cadencii.Platform.Windows
	Windows-specific audio and MIDI API
- Cadencii.Media.Dsp <- Cadencii.Platform.Windows, Cadencii.Media.Vsq
	(windows-dependent) DSP hosting API, for utau, aquestone, vocaloid etc.
- Cadencii.Media.Dsp.WindowsForms
	winforms-specific implementation for DSP (mostly for Plugin UI).

- Cadencii.Windows.Forms
	windows forms control that is specific to Cadencii.
- Cadencii.Appplication.Model
	GUI-agnostic part of the application
- Cadencii.Application.Windows.Forms
	winforms-based application, currently it is the only app.

