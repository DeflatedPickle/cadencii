WARNING
-------

This branch does NOT work fine. It is totally untested and would not
even start. Since I'm working on this on Ubuntu, nothing works
as expected and my changes are based only on builds.


Overview
--------

The project structure reorganization has a few purposes:

- decoupling Windows Forms implementation from the entire application,
  so that we can switch to other GUI frameworks. Switching to WPF is
  the first step to go, then we can try to port it to Xwt later.
- decoupling Windows-specific implementation, so that we can adopt this
  to non-Windows vocal synthesizers.
- bring (back) .NET-ism everywhere.

Currently the overall solution provides winforms-only implementation.

- Cadencii.Media.Windows offers Windows-only media capabilities.
- Cadencii.Media.Dsp.WindowsForms implements DSP plugin loader based on
  winforms and invokes Windows API.
- Cadencii.Gui.Framework.WindowsForms implements GUI Framework (which
  is actually almost direct winforms API mappings) for winforms.

Everything else is detached from winforms.

For MIDI operations, we could use managed-midi cross-platform MIDI toolkit
(once portmidi or rtmidi-c could be shipped for every platform).
Also for audio operations, we could similarly use portaudio-sharp
(once portaudio could be shipped for every platform).
Though use of existing Windows-specific API (Cadencii.Media.Windows and
Cadencii.Platform.Windows) is tightly coupled with the rest of the code,
so I would deal with them later.


Projects / Assemblies
---------------------

- Cadencii.Utilities - most common utility API
- Cadencii.Gui.Framework - awt-based API
- Cadencii.Core - fundamental API for every other library
- Cadencii.Gui.Framework.WindowsForms - winforms GUI framework implementation
- Cadencii.Platform.Windows - P/Invokes and marshals for Windows API.

- Cadencii.Media.Base - base audio and MIDI library
- Cadencii.Media.Vsq - .vsq support library
- Cadencii.Media.Windows - Windows-specific audio and MIDI API
- Cadencii.Media.Dsp - DSP API, for utau, aquestone, vocaloid etc.
- Cadencii.Media.Dsp.WindowsForms - winforms-specific DSP implementation.

- Cadencii.Windows.Forms - windows forms control that is specific to Cadencii.
- Cadencii.Appplication.Model - GUI-agnostic part of the application
- Cadencii.Application.Windows.Forms - winforms-based application.


Milestones
----------

- Replace winforms designer generated code to XML form in *.Model dll.
- Replace winforms event handler methods into UI commands and actions.
  (more code goes winforms-independent.)
- Reduce Win32-ism.
- Modify GUI framework to better fit with either WPF or Xwt.

Application Model and Implementation
------------------------------------

Current application "model" is not really so-called "model" as in MV*.
Current concern is separation of UI-framework dependent code and what not.
Once we are done with that UI fx separation, we can make smarter move,
using modern UI framework.

Right now I try to not make application logic and/or structures so that
the application would work as it used to do. It is important also because
current application doesn't work fine on Linux, where I make code changes.
If I make more "decent" changes, apps will get broken and won't work again.
