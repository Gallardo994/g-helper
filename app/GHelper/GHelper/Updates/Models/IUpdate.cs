﻿using GHelper.Helpers;

namespace GHelper.Updates.Models;

public interface IUpdate : IObservableObject
{
    public string Name { get; }
    public string Version { get; }
    public string DownloadUrl { get; }
    public bool IsNewerThanCurrent { get; }
}