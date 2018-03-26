using System;

namespace AddIn.Core
{
    public delegate void LoadAddInHandler(LoadAddInEventArgs e);

    public delegate void LoadMainFormHandler(LoadMainFormEventArgs e);

    public delegate void StartUpHandler(StartUpEventArgs e);
}
