﻿namespace GHelper.Updates.Models;

public interface IUpdate
{
    public string Name { get; }
    public string Version { get; }
    public string DownloadUrl { get; }
    public bool IsNewerThanCurrent { get; }
}