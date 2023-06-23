﻿using System;

namespace GHelper.Commands;

public interface IBackgroundCommandLoop : IDisposable
{
    public void Enqueue(IBackgroundCommand command);
    public void Enqueue(Action action);
}